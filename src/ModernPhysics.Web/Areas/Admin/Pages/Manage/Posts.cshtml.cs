using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ModernPhysics.Web.Data;
using ModernPhysics.Models;

namespace ModernPhysics.Web.Areas.Admin.Pages.Manage
{
    public class PostsModel : PageModel
    {
        private WebAppDbContext _context;
        public PostsModel(WebAppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Post> Posts { get; set; }

        public void OnGet()
        {
            Posts = _context.Posts.Include(p => p.Category);
        }

        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
            if(!ModelState.IsValid)
            {
                return new BadRequestResult();
            }

            var post = await _context.Posts.FindAsync(id);
            _context.Remove(post);
            await _context.SaveChangesAsync();

            return new RedirectToPageResult("/Admin/Posts");
        }
    }
}