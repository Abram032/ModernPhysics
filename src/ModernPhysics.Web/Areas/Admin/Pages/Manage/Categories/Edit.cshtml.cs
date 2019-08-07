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

        public class InputModel
        {
            public string Name { get; set; }
            public string FriendlyName { get; set; }
            public string Icon { get; set; }
            public bool UseCustomFriendlyName { get; set; }
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
            Input.UseCustomFriendlyName = true;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestResult();
            }

            var category = await _context.Categories.Include(p => p.Posts)
                .FirstOrDefaultAsync(p => p.Id.Equals(id));

            if (category == null)
            {
                return new BadRequestResult();
            }

            if(Input.UseCustomFriendlyName == false)
            {
                Input.FriendlyName = Input.Name.Trim().Replace(' ', '-').ToLower();
            }

            category.Name = Input.Name;
            category.FriendlyName = Input.FriendlyName;
            category.Icon = Input.Icon;

            _context.Categories.Update(category);
            await _context.SaveChangesAsync();

            return new RedirectToPageResult("/Manage/Categories", new { area = "Admin" });
        }
    }
}