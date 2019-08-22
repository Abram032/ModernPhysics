using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ModernPhysics.Web.Data;
using Microsoft.EntityFrameworkCore;
using ModernPhysics.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ModernPhysics.Web.Areas.Admin.Pages.Manage.Posts
{
    public class EditModel : PageModel
    {

        private WebAppDbContext _context;
        public EditModel(WebAppDbContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public InputModel Input { get; set; }
        public List<SelectListItem> Categories { get; set; }
        public string BaseUrl { get; set; }

        public class InputModel
        {
            public string Title { get; set; }
            public string FriendlyUrl { get; set; }
            public string Shortcut { get; set; }
            public string Content { get; set; }
            public bool IsPublished { get; set; }
            public bool UseCustomUrl { get; set; }
            public string Category { get; set; }
        }

        public async Task<IActionResult> OnGet(Guid? id)
        {
            Categories = _context.Categories
                .Select(p => new SelectListItem
                {
                    Value = p.FriendlyName,
                    Text = p.Name
                }).ToList();

            Categories.Add(new SelectListItem 
            {
                Value = null,
                Text = "Bez kategorii"
            });

            BaseUrl = $"{Request.Scheme}://{Request.Host}";

            if (id == null)
            {
                //TODO: Change returns
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id.Equals(id));

            if (post == null)
            {
                return NotFound();
            }  
            
            string categoryName = null;
            if(post.Category != null)
            {
                categoryName = post.Category.Name;
            }

            Input = new InputModel
            {
                Title = post.Title,
                FriendlyUrl = post.FriendlyUrl,
                Shortcut = Input.Shortcut,
                Content = post.Content,
                IsPublished = post.IsPublished,
                Category = categoryName
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestResult();
            }

            var post = await _context.Posts.Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id.Equals(id));

            var category = await _context.Categories
                .FirstOrDefaultAsync(p => p.FriendlyName.Equals(Input.Category));

            if(Input.UseCustomUrl == false)
            {
                Input.FriendlyUrl = Input.Title.Replace(' ','-');
            }

            post.Title = Input.Title;
            post.FriendlyUrl = Input.FriendlyUrl;
            post.Content = Input.Content;
            post.IsPublished = Input.IsPublished;
            post.Category = category;
            post.ModifiedBy = User.Identity.Name;

            if(category != null)
            {
                _context.Categories.Update(category);
            }
            _context.Posts.Update(post);

            await _context.SaveChangesAsync();

            return new RedirectToPageResult("/Manage/Posts", new { area = "Admin" });
        }
    }
}