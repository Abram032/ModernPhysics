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
        public InputModel Input { get; set; }
        public List<SelectListItem> Categories { get; set; }
        public List<SelectListItem> ContentTypes { get; set; }
        public string BaseUrl { get; set; }
        [TempData]
        public string Result { get; set; }

        public class InputModel
        {
            [Display(Name = "Tytuł *", Prompt = "Tytuł postu")]
            [Required(ErrorMessage = "Pole Tytuł jest wymagane")]
            [MaxLength(255, ErrorMessage = "Tytuł nie może być dłuższy niż 255 znaków")]
            public string Title { get; set; }
            
            [Display(Name = "Przyjazny url", Prompt = "tytul-postu (Opcjonalne)")]
            [MaxLength(255, ErrorMessage = "Przyjazny url nie może być dłuższy niż 255 znaków")]
            [RegularExpression("^[a-zA-Z0-9_-]*$", ErrorMessage = "Dozwolone są tylko duże i małe litery, cyfry, _ oraz -")]
            public string FriendlyUrl { get; set; }

            [Display(Name = "Skrót", Prompt = "Skrót postu... (Opcjonalne)")]
            [MaxLength(500, ErrorMessage = "Skrót nie może być dłuższy niż 500 znaków")]
            public string Shortcut { get; set; }

            [Required]
            [Display(Name = "Typ treści", Prompt = "Typ treści zawartości postu.")]
            public ContentType ContentType { get; set; }

            [Display(Name = "Treść strony", Prompt = "Treść strony... (Opcjonalne)")]
            [MaxLength(16777215, ErrorMessage = "Zawartość nie może być dłuższa niż 16,777,215 znaków")]
            public string Content { get; set; }
            
            [Display(Name = "Opublikuj", Prompt = "Publikuje stronę po zapisaniu.")]
            public bool IsPublished { get; set; }
            [Required]
            public string Category { get; set; }
        }

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
                Content = Input.Content,
                IsPublished = Input.IsPublished,
                Category = category,
                CreatedBy = User.Identity.Name,
                ModifiedBy = User.Identity.Name
            };

            if (category != null && category.Posts == null)
            {
                if(category.Posts == null)
                {
                    category.Posts = new List<Post>();
                }
                category.Posts.Add(post);
                _context.Categories.Update(category);
            }
                 
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
    }
}