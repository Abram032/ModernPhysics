﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ModernPhysics.Web.Data;

namespace ModernPhysics.Web.Pages
{
    public class IndexModel : PageModel
    {
        private WebAppDbContext _context;

        public IndexModel(WebAppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Models.Page> Posts { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Search { get; set; }

        public void OnGet()
        {
            Posts = _context.Pages.Include(p => p.Category)
                    .Include(p => p.PageTags)
                    .ThenInclude(p => p.Tag)
                    .Take(2);
        }
    }
}
