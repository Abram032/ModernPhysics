using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ModernPhysics.Models;
using ModernPhysics.Web.Data;

namespace ModernPhysics.Web.Areas.Admin.Pages.Manage.Posts
{
    public class AddModel : PageModel
    {
        private WebAppDbContext _context;
        public AddModel(WebAppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public string Title { get; set; }
            public string FriendlyUrl { get; set; }
            public string Tags { get; set; }
            public string Content { get; set; }
            public bool IsPublished { get; set; }
            public string Category { get; set; }
            public string CategoryIcon { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestResult();
            }

            //Category operations
            var category = _context.Categories
                .Include(p => p.Posts)
                .FirstOrDefault(p => p.Name.Equals(Input.Category));
            if (category == null)
            {
                category = new Category
                {
                    Name = Input.Category,
                    Icon = Input.CategoryIcon
                };
            }

            if (category.Posts == null)
            {
                category.Posts = new List<Post>();
            }

            //Page operations
            var post = new Post
            {
                Title = Input.Title,
                FriendlyUrl = Input.FriendlyUrl,
                Content = Input.Content,
                IsPublished = Input.IsPublished,
                Category = category,
                CreatedBy = User.Identity.Name,
                ModifiedBy = User.Identity.Name
            };

            category.Posts.Add(post);
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();

            return new RedirectToPageResult("/Admin/Posts");
        }
    }
}