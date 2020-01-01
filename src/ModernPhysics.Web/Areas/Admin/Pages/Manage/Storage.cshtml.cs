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
        public StorageModel()
        {
        }

        [TempData]
        public string ErrorMessage { get; set; }
        public string CurrentPath { get; set; }

        [BindProperty]
        public List<IFormFile> UploadFiles { get; set; }
        [BindProperty]
        public string DirectoryName { get; set; }

        public List<Models.Directory> Directories { get; set; }
        public List<Blob> Files { get; set; }

        public void OnGet(string path)
        {
            IEnumerable<string> files;
            IEnumerable<string> directories;
            Files = new List<Blob>();
            Directories = new List<Models.Directory>();
            
            if(string.IsNullOrEmpty(path))
            {
                directories = System.IO.Directory.EnumerateDirectories("content").ToList();
                files = System.IO.Directory.EnumerateFiles("content");
                CurrentPath = "content";
            }
            else
            {
                directories = System.IO.Directory.EnumerateDirectories(path).ToList();
                files = System.IO.Directory.EnumerateFiles(path);
                CurrentPath = path;
            }

            foreach(var dir in directories)
            {
                var name = dir.Replace('\\', '/').Split('/').Last();

                Directories.Add(new Models.Directory
                {
                    Name = name,
                    Path = dir,
                    CreatedAt = System.IO.Directory.GetCreationTime(dir)
                });
            }

            foreach(var file in files)
            {
                var name = file.Replace('\\', '/').Split('/').Last();
                var type = file.Split('.').Last();

                Files.Add(new Blob
                {
                    Name = name,
                    Path = file,
                    Type = type.ToLower(),
                    Url = $"https://{this.Request.Host.ToString()}/{file}",
                    CreatedAt = System.IO.File.GetCreationTime(file)
                });
            }
        }

        public async Task<IActionResult> OnPostUploadFilesAsync(string path)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            System.IO.Directory.CreateDirectory("content");

            var blobs = new List<Blob>();

            if(string.IsNullOrEmpty(path))
            {
                path = "content";
            }

            foreach (var file in UploadFiles)
            {
                var name = file.FileName.Split('\\').Last();
                var filepath = Path.Combine(path, name);

                using (var fs = System.IO.File.Create(filepath))
                {
                    await file.CopyToAsync(fs);
                }
            }

            return RedirectToPage("./Storage");
        }

        public IActionResult OnPostCreateDirectory(string path)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            
            if(string.IsNullOrEmpty(DirectoryName))
            {
                ErrorMessage = "Nazwa katalogu nie może być pusta.";
            }
            else
            {
                var dirPath = Path.Combine(path, DirectoryName);
                System.IO.Directory.CreateDirectory(dirPath);
            }

            return RedirectToPage("./Storage");
        }

        public IActionResult OnPostDeleteFileAsync(string path)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            System.IO.File.Delete(path);

            return RedirectToPage("./Storage");
        }

        public IActionResult OnPostDeleteDirectoryAsync(string path)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if(System.IO.Directory.EnumerateFileSystemEntries(path).Any() == false)
            {
                System.IO.Directory.Delete(path);
            }
            else
            {
                ErrorMessage = "Katalog nie jest pusty lub nie może zostać usunięty!";
            }

            return RedirectToPage("./Storage");
        }
    }
}