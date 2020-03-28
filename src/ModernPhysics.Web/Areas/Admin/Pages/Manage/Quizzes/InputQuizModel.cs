using ModernPhysics.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ModernPhysics.Web.Areas.Admin.Pages.Manage.Quizzes
{
    public class InputQuizModel
    {
        [Display(Name = "Tytuł *", Prompt = "Tytuł quizu")]
        public string Title { get; set; }
        [Display(Name = "Przyjazny URL", Prompt = "Przyjazny URL (Opcjonalne)")]
        public string FriendlyUrl { get; set; }
        [Display(Name = "Pytania", Prompt = "Pytania do quizu (Opcjonalne)")]
        public ICollection<Question> Questions { get; set; }
        [Display(Name = "Opublikuj", Prompt = "Publikuje stronę po zapisaniu.")]
        public bool IsPublished { get; set; }
    }
}