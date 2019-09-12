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
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ModernPhysics.Web.Areas.Admin.Pages.Manage.Categories
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

        public async Task<IActionResult> OnGet(Guid? id)
        {
            //TODO: Change returns
            if (id == null)
            {
                return new BadRequestResult();
            }

            var category = await _context.Categories.FirstOrDefaultAsync(p => p.Id.Equals(id));

            if (category == null)
            {
                return new BadRequestResult();
            }

            Input.Name = category.Name;
            Input.Icon = category.Icon;
            Input.FriendlyName = category.FriendlyName;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if(string.IsNullOrEmpty(Input.FriendlyName))
            {
                Input.FriendlyName = Regex.Replace(Input.Name, "[ !\"#$%&'()*+,./:;<=>?@[\\]^`{|}~]", "-");
            }

            if(await _context.Categories.AnyAsync(p => 
                p.FriendlyName.Equals(Input.FriendlyName) &&
                p.Id.Equals(id) == false))
            {
                Result = "Przyjazna nazwa jest już zajęta!";
                return Page();
            }

            var category = await _context.Categories.Include(p => p.Posts)
                .FirstOrDefaultAsync(p => p.Id.Equals(id));

            if (category == null)
            {
                Result = "Kategoria nie istnieje!";
                return Page();
            }

            category.Name = Input.Name;
            category.FriendlyName = Input.FriendlyName;
            category.Icon = Input.Icon;
            category.ModifiedBy = User.Identity.Name;
            //category.ModifiedAt = DateTime.UtcNow;

            _context.Categories.Update(category);
            await _context.SaveChangesAsync();

            return new RedirectToPageResult("/Manage/Categories", new { area = "Admin" });
        }
    }
}