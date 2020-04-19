using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ModernPhysics.Web.Data;
using ModernPhysics.Models;

namespace ModernPhysics.Web.Areas.Admin.Pages.Manage.Quizzes
{
    public class DeleteModel : PageModel
    {
        private WebAppDbContext _context;
        public DeleteModel(WebAppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Quiz> Quizzes { get; set; }

        public void OnGet()
        {
            Quizzes = _context.Quizzes.Where(p => p.IsDeleted == true);
        }

        public async Task<IActionResult> OnPostRestoreAsync(Guid id)
        {
            if(!ModelState.IsValid)
            {
                return new BadRequestResult();
            }

            var quiz = await _context.Quizzes.FindAsync(id);
            quiz.IsDeleted = false;
            _context.Update(quiz);
            await _context.SaveChangesAsync();

            return new RedirectToPageResult("/Manage/Quizzes/Delete", new { area = "Admin" });
        }

        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
            if(!ModelState.IsValid)
            {
                return new BadRequestResult();
            }

            var quiz = await _context.Quizzes.FindAsync(id);
            _context.Quizzes.Remove(quiz);
            await _context.SaveChangesAsync();

            return new RedirectToPageResult("/Manage/Quizzes/Deleted", new { area = "Admin" });
        }
    }
}