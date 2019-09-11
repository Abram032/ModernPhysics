using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ModernPhysics.Models;
using ModernPhysics.Web.Data;

namespace ModernPhysics.Web.Areas.Admin.Pages.Manage.Categories
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
            [Required(ErrorMessage = "Pole Nazwa jest wymagane")]
            [MaxLength(64, ErrorMessage = "Nazwa nie może być dłuższa niż 64 znaki")]
            [MinLength(1, ErrorMessage = "Nazwa nie może być krótsza niż 1 znak")]
            public string Name { get; set; }

            [Required]
            [MaxLength(64, ErrorMessage = "Przyjazna nazwa nie może być dłuższa niż 64 znaki")]
            [MinLength(1, ErrorMessage = "Przyjazna nazwa nie może być krótsza niż 1 znak")]
            [RegularExpression("^[a-zA-Z0-9_-]*$", ErrorMessage = "Dozwolone są tylko duże i małe litery, cyfry, _ oraz -")]
            public string FriendlyName { get; set; }
            [MaxLength(32, ErrorMessage = "Nazwa ikony nie może być dłuższa niż 32 znaki")]
            public string Icon { get; set; }
            public bool UseCustomFriendlyName { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestResult();
            }

            if(Input.UseCustomFriendlyName == false)
            {
                Input.FriendlyName = Input.Name.Trim().Replace(' ', '-').ToLower();
            }

            var category = new Category {
                Name = Input.Name,
                FriendlyName = Input.FriendlyName,
                Icon = Input.Icon,
                CreatedBy = User.Identity.Name,
                ModifiedBy = User.Identity.Name,
                Posts = new List<Post>()
            };

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            return new RedirectToPageResult("/Manage/Categories", new { area = "Admin" });
        }
    }
}