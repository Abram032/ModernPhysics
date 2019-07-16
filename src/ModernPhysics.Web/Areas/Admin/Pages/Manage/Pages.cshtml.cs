using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ModernPhysics.Web.Data;

namespace ModernPhysics.Web.Areas.Admin.Pages.Manage
{
    public class PagesModel : PageModel
    {
        private WebAppDbContext _context;
        public PagesModel(WebAppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Models.Page> Pages { get; set; }

        public void OnGet()
        {
            Pages = _context.Pages.Include(p => p.Category).Include(p => p.PageTags).ThenInclude(p => p.Tag);
        }

        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
            if(!ModelState.IsValid)
            {
                return new BadRequestResult();
            }

            var page = await _context.Pages.FindAsync(id);
            _context.Remove(page);
            await _context.SaveChangesAsync();

            return new RedirectToPageResult("/Admin/Pages");
        }
    }
}