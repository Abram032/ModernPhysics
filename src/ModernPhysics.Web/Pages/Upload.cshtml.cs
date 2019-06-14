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

namespace ModernPhysics.Web.Pages
{
    public class UploadModel : PageModel
    {
        WebAppDbContext _context;

        public UploadModel(WebAppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public List<IFormFile> Images { get; set; }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Directory.CreateDirectory("content");

            var blobs = new List<Blob>();

            foreach (var file in Images)
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
            return RedirectToPage("/View");
        }
    }
}