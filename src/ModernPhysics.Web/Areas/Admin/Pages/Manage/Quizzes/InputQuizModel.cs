using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ModernPhysics.Models;

namespace ModernPhysics.Web.Areas.Admin.Pages.Manage.Quizzes
{
    public class InputQuizModel
    {
        [Display(Name = "Tytuł *", Prompt = "Tytuł quizu")]
        public string Title { get; set; }
        [Display(Name = "Przyjazny URL", Prompt = "Przyjazny URL (Opcjonalne)")]
        public string FriendlyUrl { get; set; }
        [Display(Name = "Post", Prompt = "Post, który będzie miał quiz (Opcjonalne)")]
        public Guid? PostId { get; set; }
        [Display(Name = "Pytania", Prompt = "Pytania do quizu (Opcjonalne)")]
        public List<Question> Questions { get; set; }
        [Display(Name = "Opublikuj", Prompt = "Publikuje quiz po zapisaniu.")]
        public bool IsPublished { get; set; }
    }

    public class Question
    {
        public Guid Id { get; set; }
        [Display(Name = "Treść pytania", Prompt = "Treść pytania")]
        public string Text { get; set; }
        public List<Answer> Answers { get; set; }
    }

    public class Answer
    {
        public Guid Id { get; set; }
        [Display(Name = "Treść odpowiedzi", Prompt = "Treść odpowiedzi")]
        public string Text { get; set; }
        [Display(Name = "Czy poprawna", Prompt = "Czy odpowiedź jest poprawna")]
        public bool IsCorrect { get; set; }
    }
}