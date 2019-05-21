using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using ModernPhysics.HtmlParser;
using ModernPhysics.HtmlParser.Console.Models;
using Newtonsoft.Json;
using UtfUnknown;

namespace ModernPhysics.HtmlParser.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length < 1)
            {
                System.Console.WriteLine($"No path given for parser. Ex. dotnet {Assembly.GetExecutingAssembly().GetName().Name}.dll C:/path");
                return;
            }
            Stopwatch timer = new Stopwatch();
            timer.Start();

            System.Console.WriteLine("Creating directories...");
            string path = args[0];
            string jsonPath = args[1];
            string logPath = args[2];
            Directory.CreateDirectory(jsonPath);
            Directory.CreateDirectory(logPath);
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            HtmlParser parser = new HtmlParser();

            if (Directory.Exists(path) == false)
            {
                System.Console.WriteLine($"Invalid path: {path} - directory does not exist!");
                return;
            }

            System.Console.WriteLine("Getting files...");

            var files = Directory.GetFiles(path, "*.html", SearchOption.AllDirectories);

            System.Console.WriteLine($"Parsing {files.Length} files...");

            int issues = 0;
            int failures = 0;
            List<string> issueFiles = new List<string>();
            List<string> failedFiles = new List<string>();
            for(int i = 0; i < files.Length; i++)
            {
                try
                {
                    //int progress = (int)(((double)i / (double)files.Length) * 100)1;
                    //if (progress % 10 == 0)
                    if(i % 100 == 0)
                    {
                        System.Console.WriteLine($"Progress... {i}/{files.Length}");
                    }
                    var file = files[i];
                    var fileName = file.Split('\\').Last().Replace(".html", "");
                    var test = CharsetDetector.DetectFromFile(file);
                    var encodingName = CharsetDetector.DetectFromFile(file).Detected.EncodingName;
                    if (TryGetEncoding(encodingName, out var encoding) == false)
                    {
                        encoding = Encoding.UTF8;
                        issues++;
                        issueFiles.Add($"{i + 1}.json");
                    }
                    var html = File.ReadAllText(file, encoding);
                    html = parser.ConvertToUTF8(html, encoding);
                    html = parser.TagsToLower(html);
                    html = parser.RemoveLinebreaks(html);
                    var url = @"http://dydaktyka.fizyka.umk.pl" + file.Replace(path, "");
                    url = url.Replace('\\', '/');
                    Page page = new Page
                    {
                        Url = url,
                        Title = parser.GetTitle(html),
                        Content = parser.GetContent(html)
                    };
                    var json = JsonConvert.SerializeObject(page, Formatting.Indented);
                    File.WriteAllText($"{jsonPath}/{i + 1}.json", json, Encoding.UTF8);
                }
                catch
                {
                    failures++;
                    failedFiles.Add(files[i]);
                }
            }

            timer.Stop();
            StringBuilder sb = new StringBuilder();
            System.Console.WriteLine($"Parsing done!");
            System.Console.WriteLine($"Time elapsed: {timer.Elapsed}");
            System.Console.WriteLine($"Total files: {files.Length}");
            System.Console.WriteLine($"Files parsed successfully: {files.Length - issues - failures}");
            System.Console.WriteLine($"Issues during parsing: {issues}");
            System.Console.WriteLine($"Files with issues might be parsed incorrectly!");
            System.Console.WriteLine($"Files with issues saved to log issues.txt");
            foreach (var file in issueFiles)
            {
                sb.AppendLine($"{file}");
            }
            File.WriteAllText($"{logPath}/issues.txt", sb.ToString(), Encoding.UTF8);
            sb.Clear();
            System.Console.WriteLine($"Failures during parsing: {failures}");
            System.Console.WriteLine($"Files with failure were not parsed!");
            System.Console.WriteLine($"Files that failed saved to log failures.txt");
            foreach (var file in failedFiles)
            {
                sb.AppendLine($"{file}");
            }
            File.WriteAllText($"{logPath}/failures.txt", sb.ToString(), Encoding.UTF8);
        }
        
        private static bool TryGetEncoding(string encodingName, out Encoding encoding)
        {
            try
            {
                encoding = Encoding.GetEncoding(encodingName);
                return true;
            }
            catch
            {
                encoding = null;
                return false;
            }
        }
    }
}
