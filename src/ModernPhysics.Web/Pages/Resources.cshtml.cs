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
    public class ResourcesModel : PageModel
    {
        private WebAppDbContext _context;

        public ResourcesModel(WebAppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Models.Page> Posts { get; set; }
        public IEnumerable<Tag> Tags { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Search { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Category { get; set; }

        public void OnGet(string category)
        {
            if(string.IsNullOrEmpty(category))
            {
                Posts = _context.Pages.Include(p => p.Category)
                    .Include(p => p.PageTags)
                    .ThenInclude(p => p.Tag);
                Tags = _context.Set<Tag>();
            }
            else
            {
                Posts = _context.Pages.Include(p => p.Category)
                    .Include(p => p.PageTags)
                    .ThenInclude(p => p.Tag)
                    .Where(p => p.Category.Name.Equals(category));
                Tags = _context.Set<Tag>();
            }
        }

        public IActionResult OnPost(string category, string search)
        {
            if(!ModelState.IsValid)
            {
                return new BadRequestResult();
            }

            Tags = _context.Set<Tag>();

            if (!string.IsNullOrEmpty(category) && !string.IsNullOrEmpty(search))
            {
                Posts = _context.Pages.Include(p => p.Category)
                .Include(p => p.PageTags)
                .ThenInclude(p => p.Tag)
                .Where(p => p.Category.Name.Equals(category))
                .Where(p => p.Content.Contains(search));
            }

            else if(!string.IsNullOrEmpty(category) && string.IsNullOrEmpty(search))
            {
                Posts = _context.Pages.Include(p => p.Category)
                .Include(p => p.PageTags)
                .ThenInclude(p => p.Tag)
                .Where(p => p.Category.Name.Equals(category));
            }

            else if(string.IsNullOrEmpty(category) && !string.IsNullOrEmpty(search))
            {
                Posts = _context.Pages.Include(p => p.Category)
                .Include(p => p.PageTags)
                .ThenInclude(p => p.Tag)
                .Where(p => p.Content.Contains(search));
            }

            else
            {
                Posts = _context.Pages.Include(p => p.Category)
               .Include(p => p.PageTags)
               .ThenInclude(p => p.Tag);
            }

            Search = search;
            Category = category;

            return Page();
        }
    }
}