using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
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

        [TempData]
        public string Result { get; set; }

        public class InputModel
        {
            [Display(Name = "Nazwa *", Prompt = "Nazwa Kategorii")]
            [Required(ErrorMessage = "Pole jest wymagane")]
            [MaxLength(64, ErrorMessage = "Nazwa nie może być dłuższa niż 64 znaki")]
            [RegularExpression("^[a-zA-Z0-9 _-]*$", ErrorMessage = "Dozwolone są tylko duże i małe litery, cyfry, spacje, _ oraz -")]
            public string Name { get; set; }

            [Display(Name = "Przyjazna nazwa", Prompt = "Nazwa-Kategorii (Opcjonalne)")]
            [MaxLength(64, ErrorMessage = "Przyjazna nazwa nie może być dłuższa niż 64 znaki")]
            [RegularExpression("^[a-zA-Z0-9_-]*$", ErrorMessage = "Dozwolone są tylko duże i małe litery, cyfry, _ oraz -")]
            public string FriendlyName { get; set; }

            [Display(Name = "Ikona", Prompt = "fas fa-book (Opcjonalne)")]
            [MaxLength(32, ErrorMessage = "Nazwa ikony nie może być dłuższa niż 32 znaki")]
            public string Icon { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if(string.IsNullOrEmpty(Input.FriendlyName))
            {
                Input.FriendlyName = Regex.Replace(Input.Name, "[ !\"#$%&'()*+,./:;<=>?@[\\]^`{|}~]", "-");
            }

            if(await _context.Categories.AnyAsync(p => p.FriendlyName.Equals(Input.FriendlyName)))
            {
                Result = "Przyjazna nazwa jest już zajęta!";
                return Page();
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