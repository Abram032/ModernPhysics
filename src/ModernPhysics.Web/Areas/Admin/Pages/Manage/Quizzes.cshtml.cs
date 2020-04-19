using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ModernPhysics.Web.Data;
using ModernPhysics.Models;

namespace ModernPhysics.Web.Areas.Admin.Pages.Manage
{
    public class QuizzesModel : PageModel
    {
        private WebAppDbContext _context;
        public QuizzesModel(WebAppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Quiz> Quizzes { get; set; }

        public void OnGet()
        {
            Quizzes = _context.Quizzes.Where(p => p.IsDeleted == false);
        }

        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
            if(!ModelState.IsValid)
            {
                return new BadRequestResult();
            }

            var quiz = await _context.Quizzes.FindAsync(id);
            quiz.IsDeleted = true;
            _context.Update(quiz);
            await _context.SaveChangesAsync();

            return new RedirectToPageResult("/Manage/Quizzes", new { area = "Admin" });
        }
    }
}