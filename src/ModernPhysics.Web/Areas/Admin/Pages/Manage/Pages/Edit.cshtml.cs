using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ModernPhysics.Web.Data;
using ModernPhysics.Extensions;
using Microsoft.EntityFrameworkCore;
using ModernPhysics.Models;

namespace ModernPhysics.Web.Areas.Admin.Pages.Manage.Pages
{
    public class EditModel : PageModel
    {

        private WebAppDbContext _context;
        public EditModel(WebAppDbContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public Guid Id { get; set; }
            public string Title { get; set; }
            public string FriendlyUrl { get; set; }
            public string Tags { get; set; }
            public string Content { get; set; }
            public bool IsPublished { get; set; }
            public string Category { get; set; }
        }

        public async Task<IActionResult> OnGet(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var page = await _context.Pages
                .Include(p => p.Category)
                .Include(p => p.PageTags)
                .ThenInclude(p => p.Tag)
                .FirstOrDefaultAsync(p => p.Id.Equals(id));

            if (page == null)
            {
                return NotFound();
            }

            var tags = page.PageTags.Select(p => p.Tag.Name).ToArray().ToTagsString().Trim();
            Input = new InputModel
            {
                Id = (Guid)id,
                Title = page.Title,
                FriendlyUrl = page.FriendlyUrl,
                Tags = tags,
                Content = page.Content,
                IsPublished = page.IsPublished,
                Category = page.Category.Name
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestResult();
            }

            var page = await _context.Pages.Include(p => p.Category)
                .Include(p => p.PageTags)
                .ThenInclude(p => p.Tag)
                .FirstOrDefaultAsync(p => p.Id.Equals(id));
            var category = page.Category;

            //Category operations
            if (page.Category.Name != Input.Category)
            {
                category = await _context.Categories.FirstOrDefaultAsync(p => p.Name.Equals(Input.Category));
                if (category == null)
                {
                    category = new Category
                    {
                        Name = Input.Category,
                        Pages = new List<Models.Page>()
                    };
                }
                category.Pages.Add(page);
            }

            //Tag operations
            var inputTagNames = Input.Tags.Split(' ');
            var currentTagNames = page.PageTags.Select(p => p.Tag.Name).ToArray();
            var tags = new List<Tag>();

            //Removing current tags that are not in input tags
            foreach(var tagName in currentTagNames)
            {
                if(!inputTagNames.Contains(tagName))
                {
                    var tag = page.PageTags.FirstOrDefault(p => p.Tag.Name.Equals(tagName));
                    page.PageTags.Remove(tag);
                }
            }

            //Adding each tag that is not in current tags to page tags
            foreach (var tagName in inputTagNames)
            {
                if(!currentTagNames.Contains(tagName))
                {
                    var tag = _context.Tags.FirstOrDefault(p => p.Name.Equals(tagName));
                    if(tag == null)
                    {
                        tag = new Tag
                        {
                            Name = tagName
                        };
                    }

                    tags.Add(tag);
                    page.PageTags.Add(new PageTag
                    {
                        Page = page,
                        Tag = tag
                    });
                }
            }

            //Page operations
            page.Title = Input.Title;
            page.FriendlyUrl = Input.FriendlyUrl;
            page.Content = Input.Content;
            page.IsPublished = Input.IsPublished;
            page.Category = category;
            page.CreatedBy = User.Identity.Name;
            page.ModifiedBy = User.Identity.Name;

            _context.Categories.Update(category);
            _context.Tags.UpdateRange(tags);
            _context.PageTags.UpdateRange(page.PageTags);
            _context.Pages.Update(page);

            await _context.SaveChangesAsync();

            return new RedirectToPageResult("/Admin/Pages");
        }
    }
}