using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ModernPhysics.Models;
using ModernPhysics.Web.Data;

namespace ModernPhysics.Web.Areas.Admin.Pages.Manage.Pages
{
    public class AddModel : PageModel
    {
        private WebAppDbContext _context;
        public AddModel(WebAppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public string Title { get; set; }
            public string FriendlyUrl { get; set; }
            public string Tags { get; set; }
            public string Content { get; set; }
            public bool IsPublished { get; set; }
            public string Category { get; set; }
            public string CategoryIcon { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestResult();
            }

            //Tag operations
            var tags = new List<Tag>();
            var pageTags = new List<PageTag>();
            var tagNames = Input.Tags.Split(' ');
            foreach (var tagName in tagNames)
            {
                var tag = _context.Tags.Include(p => p.PageTags)
                    .ThenInclude(p => p.Page)
                    .FirstOrDefault(p => p.Name.Equals(tagName));

                if (tag != null)
                {
                    tags.Add(tag);
                }
                else
                {
                    tags.Add(new Tag
                    {
                        Name = tagName
                    });
                }
            }

            //Category operations
            var category = _context.Categories.Include(p => p.Pages).FirstOrDefault(p => p.Name.Equals(Input.Category));
            if (category == null)
            {
                category = new Category
                {
                    Name = Input.Category,
                    Icon = Input.CategoryIcon
                };
            }

            if (category.Pages == null)
            {
                category.Pages = new List<Models.Page>();
            }

            //Page operations
            var page = new Models.Page
            {
                Title = Input.Title,
                FriendlyUrl = Input.FriendlyUrl,
                Content = Input.Content,
                IsPublished = Input.IsPublished,
                Category = category,
                CreatedBy = User.Identity.Name,
                ModifiedBy = User.Identity.Name
            };

            category.Pages.Add(page);


            foreach (var tag in tags)
            {
                var _tag = _context.Tags.FirstOrDefault(p => p.Name.Equals(tag.Name));
                pageTags.Add(new PageTag
                {
                    Page = page,
                    Tag = tag
                });
            }

            _context.Categories.Update(category);
            _context.Tags.UpdateRange(tags);
            await _context.PageTags.AddRangeAsync(pageTags);
            await _context.Pages.AddAsync(page);

            await _context.SaveChangesAsync();

            return new RedirectToPageResult("/Admin/Pages");
        }
    }
}