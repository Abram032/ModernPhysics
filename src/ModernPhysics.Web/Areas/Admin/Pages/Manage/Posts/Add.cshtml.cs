using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public List<SelectListItem> Categories { get; set; }
        public string BaseUrl { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Pole Tytuł jest wymagane")]
            [MaxLength(255, ErrorMessage = "Nazwa nie może być dłuższa niż 255 znaki")]
            [MinLength(1, ErrorMessage = "Nazwa nie może być krótsza niż 1 znak")]
            public string Title { get; set; }
            
            [Required(ErrorMessage = "Pole przyjazny url jest wymagane")]
            [MaxLength(255, ErrorMessage = "Url nie może być dłuższe niż 255 znaki")]
            [MinLength(1, ErrorMessage = "Url nie może być krótszy niż 1 znak")]
            [RegularExpression("^[a-zA-Z0-9_-]*$", ErrorMessage = "Dozwolone są tylko duże i małe litery, cyfry, _ oraz -")]
            public string FriendlyUrl { get; set; }

            [MaxLength(500, ErrorMessage = "Skrót nie może być dłuższy niż 500 znaków")]
            public string Shortcut { get; set; }

            [MaxLength(16777215, ErrorMessage = "Zawartość nie może być dłuższa niż 16,777,215 znaków")]
            public string Content { get; set; }
            
            [Required]
            public bool IsPublished { get; set; }
            public bool UseCustomUrl { get; set; }
            public string Category { get; set; }
        }

        public void OnGet()
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
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if(Input.UseCustomUrl == false)
            {
                Input.FriendlyUrl = Regex.Replace(Input.Title, " !\"#$%&'()*+,./:;<=>?@[\\]^`{|}~", "-");
            }

            if (!ModelState.IsValid)
            {
                return new BadRequestResult();
            }

            var category = _context.Categories
                .Include(p => p.Posts)
                .FirstOrDefault(p => p.FriendlyName.Equals(Input.Category)); 

            if(Input.UseCustomUrl == false)
            {
                Input.FriendlyUrl = Input.Title.Replace(' ','-');
            }

            var post = new Post
            {
                Title = Input.Title,
                FriendlyUrl = Input.FriendlyUrl,
                Shortcut = Input.Shortcut,
                Content = Input.Content,
                IsPublished = Input.IsPublished,
                Category = category,
                CreatedBy = User.Identity.Name,
                ModifiedBy = User.Identity.Name
            };

            if (category != null && category.Posts == null)
            {
                if(category.Posts == null)
                {
                    category.Posts = new List<Post>();
                }
                category.Posts.Add(post);
                _context.Categories.Update(category);
            }
                 
            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();

            return new RedirectToPageResult("/Manage/Posts", new { area = "Admin" });
        }
    }
}