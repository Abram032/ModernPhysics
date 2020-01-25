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
using ModernPhysics.Web.Utils;

namespace ModernPhysics.Web.Areas.Admin.Pages.Manage.Categories
{
    public class EditModel : PageModel
    {

        private WebAppDbContext _context;
        private ICharacterParser _parser;
        public EditModel(WebAppDbContext context, ICharacterParser parser)
        {
            _context = context;
            _parser = parser;
        }

        [BindProperty(SupportsGet = true)]
        public InputCategoryModel Input { get; set; }

        [TempData]
        public string Result { get; set; }

        public async Task<IActionResult> OnGet(Guid? id)
        {
            if (id == null)
            {
                //TODO: Change into ErrorMessage and return to list
                return RedirectToPage("/NotFound");
            }

            var category = await _context.Categories.FirstOrDefaultAsync(p => p.Id.Equals(id));

            if(category.FriendlyName.Equals("No-category"))
            {
                return RedirectToPage("/Error");
            }

            if (category == null)
            {
                return RedirectToPage("/Error");
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
                Input.FriendlyName = _parser.ParsePolishChars(Input.FriendlyName);
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

            if(category.FriendlyName.Equals("No-category"))
            {
                Result = "Kategoria nie może zostać zmodyfikowana!";
                return Page();
            }

            category.Name = Input.Name;
            category.FriendlyName = Input.FriendlyName;
            category.Icon = Input.Icon;
            category.ModifiedBy = User.Identity.Name;

            _context.Categories.Update(category);
            await _context.SaveChangesAsync();

            return new RedirectToPageResult("/Manage/Categories", new { area = "Admin" });
        }
    }
}