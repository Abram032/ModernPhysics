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
    public class IndexModel : PageModel
    {

        private WebAppDbContext _context;
        public IndexModel(WebAppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Post> Posts { get; set; }

        public void OnGet()
        {
            Posts = _context.Posts.Include(p => p.Category)
                .Where(p => p.IsDeleted == false)
                .OrderByDescending(p => p.ModifiedAt)
                .Take(5);
        }

        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
            if(!ModelState.IsValid)
            {
                return new BadRequestResult();
            }

            var post = await _context.Posts.FindAsync(id);
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();

            return new RedirectToPageResult("/Manage/Posts", new { area = "Admin" });
        }
    }
}