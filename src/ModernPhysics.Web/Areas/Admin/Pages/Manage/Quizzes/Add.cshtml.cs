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
        public List<SelectListItem> Posts { get; set; }
        public string BaseUrl { get; set; }
        [TempData]
        public string Result { get; set; }

        public void OnGet()
        {
            BaseUrl = GetBaseUrl();
            Input = new InputQuizModel();

            Posts = _context.Posts
                .Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Title
                }).ToList();

            Posts.Add(new SelectListItem {
                Value = null,
                Text = "Wybierz...",
                Selected = true
            });
        }

        public async Task<IActionResult> OnPostAsync()
        {
            BaseUrl = GetBaseUrl();

            if (!ModelState.IsValid)
            {               
                return Page();
            }

            if(Input.Questions == null)
            {
                Result = "Quiz nie może być pusty";
                return Page();
            }

            if(string.IsNullOrEmpty(Input.FriendlyUrl))
            {
                Input.FriendlyUrl = Regex.Replace(Input.Title, "[ !?\"#$%&'()*+,./:;<=>@[\\]^`{|}~]", "-");
                Input.FriendlyUrl = _parser.ParsePolishChars(Input.FriendlyUrl);
            }

            if(await _context.Quizzes.AnyAsync(p => p.FriendlyUrl.Equals(Input.FriendlyUrl)))
            {
                Result = "Ten url jest już zajęty!";
                return Page();
            }

            var post = _context.Posts.FirstOrDefault(p => p.Id == Input.PostId);

            var quiz = new Models.Quiz
            {
                Title = Input.Title,
                FriendlyUrl = Input.FriendlyUrl,
                CreatedBy = User.Identity.Name,
                ModifiedBy = User.Identity.Name,
                IsPublished = Input.IsPublished,
                IsDeleted = false,
                PostId = Input.PostId,
                Post = post
            };

            var _questions = new List<Models.Question>();
            foreach(var question in Input.Questions)
            {
                var _question = new Models.Question
                {
                    Text = question.Text,
                    Answers = new List<Models.Answer>(),
                    Quiz = quiz
                };                

                foreach(var answer in question.Answers)
                {
                    var _answer = new Models.Answer
                    {
                        Text = answer.Text,
                        IsCorrect = answer.IsCorrect,
                        Question = _question
                    };
                    _question.Answers.Add(_answer);
                }
                _questions.Add(_question);
            }

            quiz.Questions = _questions;

            await _context.Quizzes.AddAsync(quiz);      
            await _context.SaveChangesAsync();

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