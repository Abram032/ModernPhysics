using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ModernPhysics.Explorer.Pages
{
    public class IndexModel : PageModel
    {
        public class PageInfo
        {
            public string Url { get; set; }
            public string Title { get; set; }
            public string Content { get; set; }
        }

        [BindProperty]
        public List<IFormFile> Files { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }

        public List<PageInfo> PageInfos { get; set; }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnGetSearchAsync(string searchString)
        {
            PageInfos = new List<PageInfo>();
            HttpClient client = new HttpClient();

            var query = "{\"query\": {\"match\": {\"Content\": \"" + searchString + "\"}}}";
            var response = await client.SendAsync(
                new HttpRequestMessage
                {
                    Content = new StringContent(query, Encoding.UTF8, "application/json"),
                    RequestUri = new Uri("http://localhost:9200/pages/_search?pretty"),
                    Method = HttpMethod.Get
                });
            var json = await response.Content.ReadAsStringAsync();
            var data = JObject.Parse(json);
            var pages = JArray.Parse(data["hits"]["hits"].ToString());

            foreach(var page in pages)
            {
                var _page = JObject.Parse(page.ToString());
                var pageInfo = JsonConvert.DeserializeObject<PageInfo>(_page["_source"].ToString());
                pageInfo.Content = new string(pageInfo.Content.Take(50).ToArray());
                pageInfo.Content += "...";
                PageInfos.Add(pageInfo);
            }
            
            SearchString = searchString;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            HttpClient client = new HttpClient();
            var response = await client.SendAsync(
                new HttpRequestMessage
                {
                    RequestUri = new Uri("http://localhost:9200/pages"),
                    Method = HttpMethod.Head
                });

            if(response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                response = await client.SendAsync(
                    new HttpRequestMessage
                    {
                        RequestUri = new Uri("http://localhost:9200/pages?pretty"),
                        Method = HttpMethod.Put
                    });
                if (response.IsSuccessStatusCode == false)
                {
                    return RedirectToPage("/Error");
                }
            }

            foreach (var file in Files)
            {
                var reader = new StreamReader(file.OpenReadStream());
                var content = new StringContent(reader.ReadToEnd(), Encoding.UTF8, "application/json");
                response = await client.PostAsync("http://localhost:9200/pages/_doc?pretty", content);
            }

            return RedirectToPage("/Index");
        }
    }
}
