using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ModernPhysics.Models;
using ModernPhysics.Web.Data;

namespace ModernPhysics.Web.Areas.Admin.Pages.Manage.Posts
{
    public class AddModel : PageModel
    {
        private WebAppDbContext _context;
        public AddModel(WebAppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public InputPostModel Input { get; set; }
        public List<SelectListItem> Categories { get; set; }
        public List<SelectListItem> ContentTypes { get; set; }
        public string BaseUrl { get; set; }
        [TempData]
        public string Result { get; set; }

        public void OnGet()
        {
            Categories = GetCategories();
            BaseUrl = GetBaseUrl();
            ContentTypes = GetContentTypes();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Categories = GetCategories();
            BaseUrl = GetBaseUrl();
            ContentTypes = GetContentTypes();

            if (!ModelState.IsValid)
            {               
                return Page();
            }

            if(string.IsNullOrEmpty(Input.FriendlyUrl))
            {
                Input.FriendlyUrl = Regex.Replace(Input.Title, "[ !\"#$%&'()*+,./:;<=>?@[\\]^`{|}~]", "-");
            }

            if(await _context.Posts.AnyAsync(p =>
                p.FriendlyUrl.Equals(Input.FriendlyUrl) &&
                p.Category.Name.Equals(Input.Category)))
                {
                    Result = "Ten url jest już zajęty!";
                    return Page();
                }

            var category = _context.Categories
                .Include(p => p.Posts)
                .FirstOrDefault(p => p.FriendlyName.Equals(Input.Category)); 

            var post = new Post
            {
                Title = Input.Title,
                FriendlyUrl = Input.FriendlyUrl,
                Shortcut = Input.Shortcut,
                ContentType = Input.ContentType,
                Content = SanitizeHtml(Input.Content),
                IsPublished = Input.IsPublished,
                Category = category,
                CreatedBy = User.Identity.Name,
                ModifiedBy = User.Identity.Name,
                IsDeleted = false
            };

            await _context.Posts.AddAsync(post);      
            await _context.SaveChangesAsync();

            return new RedirectToPageResult("/Manage/Posts", new { area = "Admin" });
        }

        private string GetBaseUrl() => $"{Request.Scheme}://{Request.Host}";

        private List<SelectListItem> GetCategories()
        {
            var list = new List<SelectListItem>();

            list = _context.Categories
                .Select(p => new SelectListItem
                {
                    Value = p.FriendlyName,
                    Text = p.Name
                }).ToList();

            return list;
        }

        private List<SelectListItem> GetContentTypes()
        {
            var list = Enum.GetValues(typeof(ContentType))
                .Cast<ContentType>()
                .Select(t => new SelectListItem {
                    Text = t.ToString(),
                    Value = ((int)t).ToString()
                    }).ToList();

            return list;
        }

        private string ReplaceRegex(string source, string regex, string replaceWith)
        {
            var matches = Regex.Matches(source, regex);
            for (int i = 0; i < matches.Count; i++)
            {
                source = source.Replace(matches[i].Value, replaceWith);
            }
            return source;
        }

        private string SanitizeHtml(string html)
        {
            html = ReplaceRegex(html, @"<script(.*?)>(.*?)</script>", "");
            html = ReplaceRegex(html, @"<object(.*?)>(.*?)</object>", "");
            html = ReplaceRegex(html, @"<link(.*?)>(.*?)</link>", "");
            html = ReplaceRegex(html, @"<embed(.*?)>(.*?)</embed>", "");
            return html;
        }
    }
}