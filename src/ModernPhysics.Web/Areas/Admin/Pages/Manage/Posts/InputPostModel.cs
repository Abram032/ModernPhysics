using ModernPhysics.Models;
using System.ComponentModel.DataAnnotations;

namespace ModernPhysics.Web.Areas.Admin.Pages.Manage.Posts
{
    public class InputPostModel
    {
        [Display(Name = "Tytuł *", Prompt = "Tytuł postu")]
        public string Title { get; set; }
        [Display(Name = "Przyjazny URL", Prompt = "tytul-postu (Opcjonalne)")]
        public string FriendlyUrl { get; set; }
        [Display(Name = "Skrót", Prompt = "Skrót postu... (Opcjonalne)")]
        public string Shortcut { get; set; }
        [Display(Name = "Typ treści *", Prompt = "Typ treści zawartości postu.")]
        public ContentType ContentType { get; set; } 
        [Display(Name = "Treść strony", Prompt = "Treść strony... (Opcjonalne)")]
        public string Content { get; set; }
        [Display(Name = "Opublikuj", Prompt = "Publikuje stronę po zapisaniu.")]
        public bool IsPublished { get; set; }
        [Display(Name = "Kategoria *")]
        public string Category { get; set; }
    }
}