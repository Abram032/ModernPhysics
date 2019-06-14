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

            Directory.CreateDirectory("/mnt");
            Directory.CreateDirectory("/mnt/content");

            var blobs = new List<Blob>();

            foreach (var file in Images)
            {
                var blob = new Blob
                {
                    Name = file.FileName,
                    Path = "/app/mnt/content/" + file.FileName,
                    Type = file.ContentType,
                    Url = this.Request.Host.ToString() + "/mnt/content/" + file.FileName
                };
                blobs.Add(blob);

                using (FileStream fs = new FileStream(blob.Path, FileMode.Create))
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