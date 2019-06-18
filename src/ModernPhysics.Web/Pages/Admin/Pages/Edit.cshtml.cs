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

namespace ModernPhysics.Web.Pages.Admin.Pages
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
            var pageTags = page.PageTags;

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
            var actualTagNames = pageTags.Select(p => p.Tag.Name).ToArray();
            var newPageTags = new List<PageTag>();
            var tags = new List<Tag>();
            foreach (var inputTagName in inputTagNames)
            {
                if (actualTagNames.Contains(inputTagName))
                {
                    newPageTags.Add(pageTags.FirstOrDefault(p => p.PageId.Equals(id)));
                }
                else
                {
                    var tag = await _context.Tags.Include(p => p.PageTags)
                        .FirstOrDefaultAsync(p => p.Name.Equals(inputTagName));
                    if (tag != null)
                    {
                        tags.Add(tag);
                    }
                    else
                    {
                        tags.Add(new Tag
                        {
                            Name = inputTagName
                        });
                    }
                }
            }

            foreach (var tag in tags)
            {
                newPageTags.Add(new PageTag
                {
                    Page = page,
                    Tag = tag
                });
            }
            pageTags = newPageTags;

            //Page operations
            page.Title = Input.Title;
            page.FriendlyUrl = Input.FriendlyUrl;
            page.Content = Input.Content;
            page.IsPublished = Input.IsPublished;
            page.Category = category;
            page.CreatedBy = User.Identity.Name;
            page.ModifiedBy = User.Identity.Name;
            page.PageTags = pageTags;

            _context.Categories.Update(category);
            _context.Tags.UpdateRange(tags);
            _context.PageTags.UpdateRange(pageTags);
            _context.Pages.Update(page);

            await _context.SaveChangesAsync();

            return new RedirectToPageResult("/Admin/Pages");
        }
    }
}