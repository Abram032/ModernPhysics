using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ModernPhysics.Models;
using ModernPhysics.Web.Data;
using ModernPhysics.Extensions;

namespace ModernPhysics.Web.Pages
{
    public class AddModel : PageModel
    {
        private WebAppDbContext _context;
        public AddModel(WebAppDbContext context)
        {
            _context = context;
        }


        public ICollection<Models.Page> Pages { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public string Title { get; set; }
            public string FriendlyUrl { get; set; }
            public string Tags { get; set; }
            public string Content { get; set; }
            public bool IsPublished { get; set; }
        }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync()
        {
            if(!ModelState.IsValid)
            {
                return new BadRequestResult();
            }

            var tags = new List<Tag>();
            var pageTags = new List<PageTag>();
            tags.AddRange(Input.Tags.Split(' ').ToTags());

            var page = new Models.Page
            {
                Title = Input.Title,
                FriendlyUrl = Input.FriendlyUrl,
                Content = Input.Content,
                IsPublished = Input.IsPublished,
                CreatedBy = "admin"
                //PageTags = pageTags
            };

            foreach(var tag in tags)
            {
                //tag.PageTags = pageTags;
                pageTags.Add(new PageTag
                {
                    Page = page,
                    Tag = tag
                });
            }

            await _context.Pages.AddAsync(page);
            await _context.Tags.AddRangeAsync(tags);
            await _context.PageTags.AddRangeAsync(pageTags);

            await _context.SaveChangesAsync();

            return new RedirectToPageResult("/View");
        }   
    }
}