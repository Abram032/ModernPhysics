using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ModernPhysics.Web.Data;
using ModernPhysics.Models;

namespace ModernPhysics.Web.Areas.Admin.Pages.Manage.Posts
{
    public class DeletedModel : PageModel
    {
        private WebAppDbContext _context;
        public DeletedModel(WebAppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Post> Posts { get; set; }

        public void OnGet()
        {
            Posts = _context.Posts.Include(p => p.Category)
                .Where(p => p.IsDeleted == true);
        }

        public async Task<IActionResult> OnPostRestoreAsync(Guid id)
        {
            if(!ModelState.IsValid)
            {
                return new BadRequestResult();
            }

            var post = await _context.Posts.FindAsync(id);
            post.IsDeleted = false;
            _context.Update(post);
            await _context.SaveChangesAsync();

            return new RedirectToPageResult("/Manage/Posts/Deleted", new { area = "Admin" });
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

            return new RedirectToPageResult("/Manage/Posts/Deleted", new { area = "Admin" });
        }
    }
}