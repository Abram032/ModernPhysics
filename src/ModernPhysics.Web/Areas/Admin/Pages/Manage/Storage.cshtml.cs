using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ModernPhysics.Models;
using ModernPhysics.Web.Data;

namespace ModernPhysics.Web.Areas.Admin.Pages.Manage
{
    public class StorageModel : PageModel
    {
        private WebAppDbContext _context;

        public StorageModel(WebAppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public List<IFormFile> Files { get; set; }

        public IEnumerable<Blob> Blobs { get; set; }

        public void OnGet()
        {
            Blobs = _context.Blobs;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Directory.CreateDirectory("content");

            var blobs = new List<Blob>();

            foreach (var file in Files)
            {
                var name = file.FileName.Split('\\').Last();
                var path = Path.Combine("content", name);
                var blob = new Blob
                {
                    Name = name,
                    Path = path,
                    Type = file.ContentType,
                    Url = this.Request.Host.ToString() + "/content/" + name
                };
                blobs.Add(blob);

                using (var fs = System.IO.File.Create(blob.Path))
                {
                    await file.CopyToAsync(fs);
                }
            }

            _context.Blobs.UpdateRange(blobs);
            await _context.SaveChangesAsync();
            return RedirectToPage("./Storage");
        }

        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var blob = await _context.Blobs.FindAsync(id);
            _context.Remove(blob);
            await _context.SaveChangesAsync();
            return RedirectToPage("./Storage");
        }
    }
}