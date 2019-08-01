using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ModernPhysics.Models;
using ModernPhysics.Web.Data;

namespace ModernPhysics.Web.Areas.Admin.Pages.Manage
{
    public class CategoriesModel : PageModel
    {
        private WebAppDbContext _context;
        public CategoriesModel(WebAppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Category> Categories { get; set; }

        public void OnGet()
        {
            Categories = _context.Categories;
        }

        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestResult();
            }

            var category = await _context.Categories.Include(p => p.Posts)
                .FirstOrDefaultAsync(p => p.Id.Equals(id));

            var nocategory = await _context.Categories.Include(p => p.Posts)
                .FirstOrDefaultAsync(p => p.Name.Equals("no-category"));

            if (nocategory == null)
            {
                nocategory = new Category
                {
                    Name = "No category",
                    FriendlyName = "no-category",
                    Icon = "fas fa-question"
                };
            }

            foreach(var post in category.Posts)
            {
                post.Category = nocategory;
            }

            _context.Remove(category);
            _context.Categories.Update(nocategory);
            _context.Posts.UpdateRange(category.Posts);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Categories");
        }
    }
}