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
        public InputCategoryModel Input { get; set; }

        [TempData]
        public string Result { get; set; }

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