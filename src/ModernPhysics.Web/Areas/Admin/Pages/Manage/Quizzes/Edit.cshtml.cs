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
    public class EditModel : PageModel
    {
        private WebAppDbContext _context;
        private ICharacterParser _parser;
        public EditModel(WebAppDbContext context, ICharacterParser parser)
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

        public async Task<IActionResult> OnGet(Guid? id)
        {
            if (id == null)
            {
                return RedirectToPage("/NotFound");
            }

            Posts = _context.Posts
                .Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Title
                }).ToList();

            Posts.Add(new SelectListItem {
                Value = null,
                Text = "Wybierz..."
            });

            var quiz = await _context.Quizzes
                .Include(p => p.Post)
                .Include(p => p.Questions)
                .ThenInclude(p => p.Answers)
                .Where(p => p.IsDeleted == false)
                .FirstOrDefaultAsync(p => p.Id.Equals(id));

            if (quiz == null)
            {
                return RedirectToPage("/NotFound");
            }

            if(quiz.Post != null)
            {
                var item = Posts.FirstOrDefault(p => p.Value.Equals(quiz.Post.Id.ToString()));
                item.Selected = true;
            }
            else
            {
                var item = Posts.FirstOrDefault(p => p.Value == null);
                item.Selected = true;
            }

            BaseUrl = GetBaseUrl();

            var questions = new List<Question>();
            if(quiz.Questions != null)
            {
                foreach(var question in quiz.Questions)
                {
                    var answers = new List<Answer>();
                    if(question.Answers != null)
                    {
                        foreach(var answer in question.Answers)
                        {
                            answers.Add(new Answer
                            {
                                Id = answer.Id,
                                Text = answer.Text,
                                IsCorrect = answer.IsCorrect
                            });
                        }
                    }
                    questions.Add(new Question 
                    {
                        Id = question.Id,
                        Text = question.Text,
                        Answers = answers
                    });
                }
            }

            Input = new InputQuizModel
            {
                Title = quiz.Title,
                FriendlyUrl = quiz.FriendlyUrl,
                IsPublished = quiz.IsPublished,
                Questions = questions
            };
            
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid id)
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

            if(await _context.Quizzes.AnyAsync(p => p.FriendlyUrl.Equals(Input.FriendlyUrl) && p.Id.Equals(id) == false))
            {
                Result = "Ten url jest już zajęty!";
                return Page();
            }

            var quiz = await _context.Quizzes.Include(p => p.Questions)
                .ThenInclude(p => p.Answers)
                .FirstOrDefaultAsync(p => p.Id.Equals(id));
                
            var post = _context.Posts.FirstOrDefault(p => p.Id == Input.PostId);

            quiz.Title = Input.Title;
            quiz.FriendlyUrl = Input.FriendlyUrl;
            quiz.IsPublished = Input.IsPublished;
            quiz.ModifiedBy = User.Identity.Name;
            quiz.PostId = Input.PostId;
            quiz.Post = post;

            var _questions = new List<Models.Question>();
            foreach(var question in Input.Questions)
            {
                var _question = new Models.Question
                {
                    Id = question.Id,
                    Text = question.Text,
                    Answers = new List<Models.Answer>(),
                    Quiz = quiz
                };                

                foreach(var answer in question.Answers)
                {
                    var _answer = new Models.Answer
                    {
                        Id = answer.Id,
                        Text = answer.Text,
                        IsCorrect = answer.IsCorrect,
                        Question = _question
                    };
                    _question.Answers.Add(_answer);
                }
                _questions.Add(_question);
            }

            quiz.Questions = _questions;

            _context.Quizzes.Update(quiz);      
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