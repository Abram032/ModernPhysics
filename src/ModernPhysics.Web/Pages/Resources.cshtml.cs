using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        public IEnumerable<Post> Posts { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Search { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Category { get; set; }

        public List<SelectListItem> Categories { get; set; }

        //? UNDONE: Should i use tracable searches? (query in url)

        public void OnGet(string category)
        {
            Categories = _context.Categories
                .Select(p => new SelectListItem
                {
                    Value = p.FriendlyName,
                    Text = p.Name
                }).ToList();

            Categories.Add(new SelectListItem
            {
                Value = null,
                Text = "Wszystkie"
            });

            if(string.IsNullOrEmpty(category))
            {
                Posts = _context.Posts.Include(p => p.Category)
                    .Where(p => p.IsPublished == true);

                Categories.FirstOrDefault(p => p.Text.Equals("Wszystkie")).Selected = true;
            }
            else
            {
                Posts = _context.Posts.Include(p => p.Category)
                    .Where(p => p.Category.FriendlyName.Equals(category))
                    .Where(p => p.IsPublished == true);

                Categories.FirstOrDefault(p => p.Value.Equals(category)).Selected = true;
            }
            
            Category = category;
        }

        //TODO: Add searching after titles

        public IActionResult OnPost(string category, string search)
        {
            if(!ModelState.IsValid)
            {
                return new BadRequestResult();
            }

            Categories = _context.Categories
                .Select(p => new SelectListItem
                {
                    Value = p.FriendlyName,
                    Text = p.Name
                }).ToList();

            Categories.Add(new SelectListItem
            {
                Value = null,
                Text = "Wszystkie"
            });

            //Search everything
            if((string.IsNullOrEmpty(category) || category.Equals("Wszystkie")) 
                && string.IsNullOrEmpty(search))
            {
                Posts = _context.Posts.Include(p => p.Category)
                    .Where(p => p.IsPublished == true);

                Categories.FirstOrDefault(p => p.Text.Equals("Wszystkie")).Selected = true;
            }

            //Search *string* in everything
            else if((string.IsNullOrEmpty(category) || category.Equals("Wszystkie")) 
                && !string.IsNullOrEmpty(search))
            {
                Posts = _context.Posts.Include(p => p.Category)
                    .Where(p => p.Title.Contains(search) || p.Content.Contains(search))
                    .Where(p => p.IsPublished == true);

                Categories.FirstOrDefault(p => p.Text.Equals("Wszystkie")).Selected = true;
            }

            //Search everything in *category*
            else if(!string.IsNullOrEmpty(category) && string.IsNullOrEmpty(search))
            {
                Posts = _context.Posts.Include(p => p.Category)
                    .Where(p => p.Category.FriendlyName.Equals(category))
                    .Where(p => p.IsPublished == true);

                Categories.FirstOrDefault(p => p.Value.Equals(category)).Selected = true;
            }
            
            //Search *string* in *category*
            else
            {
                Posts = _context.Posts.Include(p => p.Category)
                    .Where(p => p.Category.FriendlyName.Equals(category))
                    .Where(p => p.Title.Contains(search) || p.Content.Contains(search))
                    .Where(p => p.IsPublished == true);

                Categories.FirstOrDefault(p => p.Value.Equals(category)).Selected = true;
            }

            Search = search;
            Category = category;

            return Page();
        }
    }
}