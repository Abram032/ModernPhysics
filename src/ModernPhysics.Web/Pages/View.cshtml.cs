using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ModernPhysics.Models;
using ModernPhysics.Web.Data;

namespace ModernPhysics.Web.Pages
{
    public class ViewModel : PageModel
    {
        private WebAppDbContext _context;
        public ViewModel(WebAppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Models.Page> Pages { get; set; }
        public IEnumerable<Tag> Tags { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Blob> Blobs { get; set; }

        public void OnGet()
        {
            Pages = _context.Set<Models.Page>().Include(p => p.PageTags).ThenInclude(p => p.Tag);
            Tags = _context.Set<Tag>().Include(p => p.PageTags).ThenInclude(p => p.Page);
            Categories = _context.Set<Category>().Include(p => p.Pages);
            Blobs = _context.Set<Blob>();
        }
    }
}