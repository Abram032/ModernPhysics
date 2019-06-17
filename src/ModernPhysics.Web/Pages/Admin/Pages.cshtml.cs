using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ModernPhysics.Web.Data;

namespace ModernPhysics.Web.Pages.Admin
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
    }
}