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
using ModernPhysics.Web.Utils;

namespace ModernPhysics.Web.Areas.Admin.Pages.Manage.Quizzes
{
    public class AddModel : PageModel
    {
        private WebAppDbContext _context;
        private ICharacterParser _parser;
        public AddModel(WebAppDbContext context, ICharacterParser parser)
        {
            _context = context;
            _parser = parser;
        }

        [BindProperty]
        public InputQuizModel Input { get; set; }
        public string BaseUrl { get; set; }
        [TempData]
        public string Result { get; set; }

        public void OnGet()
        {
            BaseUrl = GetBaseUrl();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // BaseUrl = GetBaseUrl();

            // if (!ModelState.IsValid)
            // {               
            //     return Page();
            // }

            // if(string.IsNullOrEmpty(Input.FriendlyUrl))
            // {
            //     Input.FriendlyUrl = Regex.Replace(Input.Title, "[ !?\"#$%&'()*+,./:;<=>@[\\]^`{|}~]", "-");
            //     Input.FriendlyUrl = _parser.ParsePolishChars(Input.FriendlyUrl);
            // }

            // if(await _context.Posts.AnyAsync(p =>
            //     p.FriendlyUrl.Equals(Input.FriendlyUrl) &&
            //     p.Category.Name.Equals(Input.Category)))
            //     {
            //         Result = "Ten url jest już zajęty!";
            //         return Page();
            //     }

            // var category = _context.Categories
            //     .Include(p => p.Posts)
            //     .FirstOrDefault(p => p.FriendlyName.Equals(Input.Category)); 

            // var post = new Post
            // {
            //     Title = Input.Title,
            //     FriendlyUrl = Input.FriendlyUrl,
            //     Shortcut = Input.Shortcut,
            //     ContentType = Input.ContentType,
            //     Content = SanitizeHtml(Input.Content),
            //     IsPublished = Input.IsPublished,
            //     Category = category,
            //     CreatedBy = User.Identity.Name,
            //     ModifiedBy = User.Identity.Name,
            //     IsDeleted = false
            // };

            // await _context.Posts.AddAsync(post);      
            // await _context.SaveChangesAsync();

            return new RedirectToPageResult("/Manage/Quizzes", new { area = "Admin" });
        }

        private string GetBaseUrl() => $"{Request.Scheme}://{Request.Host}";

        private string ReplaceRegex(string source, string regex, string replaceWith)
        {
            var matches = Regex.Matches(source, regex);
            for (int i = 0; i < matches.Count; i++)
            {
                source = source.Replace(matches[i].Value, replaceWith);
            }
            return source;
        }
    }
}