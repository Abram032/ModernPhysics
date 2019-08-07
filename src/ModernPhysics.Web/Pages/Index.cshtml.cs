using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ModernPhysics.Web.Data;
using ModernPhysics.Models;

namespace ModernPhysics.Web.Pages
{
    public class IndexModel : PageModel
    {
        private WebAppDbContext _context;

        public IndexModel(WebAppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Post> Posts { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Search { get; set; }

        public void OnGet()
        {
            Posts = _context.Posts.Include(p => p.Category).
                Where(p => p.IsPublished == true)
                .Take(2);
        }
    }
}
