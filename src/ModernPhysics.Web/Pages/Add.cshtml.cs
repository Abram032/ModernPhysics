using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ModernPhysics.Models;
using ModernPhysics.Web.Data;
using ModernPhysics.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace ModernPhysics.Web.Pages
{
    [Authorize]
    public class AddModel : PageModel
    {
        private WebAppDbContext _context;

        public AddModel(WebAppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public InputPageModel InputPage { get; set; }
        [BindProperty]
        public InputBlobModel InputBlob { get; set; }

        public class InputPageModel
        {
            public string Title { get; set; }
            public string FriendlyUrl { get; set; }
            public string Tags { get; set; }
            public string Content { get; set; }
            public bool IsPublished { get; set; }
            public string Category { get; set; }
        }
        
        public class InputBlobModel
        {
            public string Name { get; set; }
            public string Path { get; set; }
            public string Url { get; set; }
            public string Type { get; set; }
            public string Description { get; set; }
        }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostPageAsync()
        {
            if(!ModelState.IsValid)
            {
                return new BadRequestResult();
            }
            
            //Tag operations
            var tags = new List<Tag>();
            var pageTags = new List<PageTag>();
            var tagNames = InputPage.Tags.Split(' ');
            foreach (var tagName in tagNames)
            {
                var tag = _context.Tags.Include(p => p.PageTags)
                    .ThenInclude(p => p.Page)
                    .FirstOrDefault(p => p.Name.Equals(tagName));

                if(tag != null)
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
            var category = _context.Categories.Include(p => p.Pages).FirstOrDefault(p => p.Name.Equals(InputPage.Category));
            if (category == null)
            {
                category = new Category
                {
                    Name = InputPage.Category
                };
            }

            if (category.Pages == null)
            {
                category.Pages = new List<Models.Page>();
            }

            //Page operations
            var page = new Models.Page
            {
                Title = InputPage.Title,
                FriendlyUrl = InputPage.FriendlyUrl,
                Content = InputPage.Content,
                IsPublished = InputPage.IsPublished,
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

            return new RedirectToPageResult("/View");
        }

        public async Task<IActionResult> OnPostBlobAsync()
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestResult();
            }

            var url = BuildUrl(InputBlob.Path, InputBlob.Name, InputBlob.Type);
            var blob = _context.Blobs.FirstOrDefault(p => p.Url.Equals(url));

            if(blob == null)
            {
                blob = new Blob();
            }

            blob.Name = InputBlob.Name;
            blob.Path = InputBlob.Path;
            blob.Type = InputBlob.Type;
            blob.Description = InputBlob.Description;
            blob.Url = url;

            _context.Blobs.Update(blob);
            await _context.SaveChangesAsync();

            return new RedirectToPageResult("/View");
        }

        private string BuildUrl(string path, string name, string type)
        {
            path = EnsurePrecedingSlash(path);
            path = EnsureSubsequentSlash(path);
            type = EnsurePrecedingDot(type);
            return path + name + type;
        }

        private string EnsurePrecedingSlash(string value)
        {
            var c = value.FirstOrDefault();
            if(c.Equals('/'))
            {
                return value;
            }
            return '/' + value;
        }

        private string EnsureSubsequentSlash(string value)
        {
            var c = value.Last();
            if (c.Equals('/'))
            {
                return value;
            }
            return value + '/';
        }

        private string EnsurePrecedingDot(string value)
        {
            var c = value.FirstOrDefault();
            if(c.Equals('.'))
            {
                return value;
            }
            return '.' + value;
        }
    }
}