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
        [BindProperty(SupportsGet = true)]
        public string RootPath { get; set; }

        [BindProperty]
        public List<IFormFile> UploadFiles { get; set; }
        [BindProperty]
        public string DirectoryName { get; set; }

        public List<Models.Directory> Directories { get; set; }
        public List<Blob> Files { get; set; }

        public IActionResult OnGet(string path)
        {
            List<string> files;
            List<string> directories;
            Files = new List<Blob>();
            Directories = new List<Models.Directory>();
            
            if(string.IsNullOrEmpty(path) || path.Equals("content"))
            {
                directories = System.IO.Directory.EnumerateDirectories("content").ToList();
                files = System.IO.Directory.EnumerateFiles("content").ToList();
                CurrentPath = "content";
                RootPath = null;
            }
            else
            {
                //Sanitizing route
                path = path.Replace('\\', '/');
                
                if(path.Contains("/.."))
                {
                    return RedirectToPage("./NotFound");
                }

                if(System.IO.Directory.Exists(path) == false)
                {
                    return RedirectToPage("./NotFound");
                }
                directories = System.IO.Directory.EnumerateDirectories(path).ToList();
                files = System.IO.Directory.EnumerateFiles(path).ToList();
                CurrentPath = path;
                RootPath = path.Replace("/" + path.Split('/').Last(), "");
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

            return Page();
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

            return RedirectToPage("./Storage", new { path = path });
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

            return RedirectToPage("./Storage", new { path = path });
        }

        public IActionResult OnPostDeleteFileAsync(string path, string filename)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            System.IO.File.Delete(Path.Combine(path, filename));

            return RedirectToPage("./Storage", new { path = path });
        }

        public IActionResult OnPostDeleteDirectoryAsync(string path, string dirname)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if(System.IO.Directory.EnumerateFileSystemEntries(Path.Combine(path, dirname)).Any() == false)
            {
                System.IO.Directory.Delete(Path.Combine(path, dirname));
            }
            else
            {
                ErrorMessage = "Katalog nie jest pusty lub nie może zostać usunięty!";
            }

            return RedirectToPage("./Storage", new { path = path });
        }
    }
}