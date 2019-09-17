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
    public class PostModel : PageModel
    {
        private WebAppDbContext _context;

        public PostModel(WebAppDbContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public string Category { get; set; }
        [BindProperty(SupportsGet = true)]
        public string PostUrl { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Search { get; set; }

        public Post Post { get; set; }
        public List<Category> Categories { get; set; }

        public IActionResult OnGet(string category, string posturl)
        {
            
            Post = _context.Posts
                .Include(p => p.Category)
                .FirstOrDefault(p => p.Category.FriendlyName.Equals(category) && 
                    p.FriendlyUrl.Equals(posturl));

            if(Post == null)
            {
                return RedirectToPage("/Error");
            }

            if(Post.IsPublished == false)
            {
                //TODO: Change to redirect to Not Found page
                return RedirectToPage("/Error");
            }

            Categories = _context.Categories.Include(p => p.Posts).ToList();
            foreach(var _category in Categories)
            {
                _category.Posts = _category.Posts.Where(p => p.IsPublished == true).ToList();
            }
            
            return Page();
        }
    }
}