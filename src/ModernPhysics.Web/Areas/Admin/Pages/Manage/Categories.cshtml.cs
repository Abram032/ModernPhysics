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
    //TODO: Add author and date created/updated
    public class CategoriesModel : PageModel
    {
        private WebAppDbContext _context;
        public CategoriesModel(WebAppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Category> Categories { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

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

            var noCategory = await _context.Categories
                .FirstOrDefaultAsync(p => p.FriendlyName.Equals("No-category"));

            if(category == null || noCategory == null || category.FriendlyName.Equals("No-category"))
            {
                ErrorMessage = "Coś poszło nie tak, kategoria nie istnieje lub nie można jej usunąć";
                return RedirectToPage("./Categories");
            }
            
            foreach(var post in category.Posts)
            {
                post.Category = noCategory;
            }

            _context.Remove(category);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Categories");
        }
    }
}