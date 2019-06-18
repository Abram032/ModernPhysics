using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ModernPhysics.Models;
using ModernPhysics.Web.Data;

namespace ModernPhysics.Web.Pages.Admin
{
    public class TagsModel : PageModel
    {
        private WebAppDbContext _context;
        public TagsModel(WebAppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Tag> Tags { get; set; }

        public void OnGet()
        {
            Tags = _context.Tags;
        }

        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestResult();
            }

            var tag = await _context.Tags.FindAsync(id);
            _context.Remove(tag);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Tags");
        }
    }
}