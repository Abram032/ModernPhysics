using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ModernPhysics.HtmlParser
{
    public class HtmlParser
    {
        public string GetTitle (string html)
        {
            string title = "";
            html = RemoveLinebreaks(html);

            if(html.Contains("<title>"))
            {
                var match = Regex.Match(html, "<title>(.*?)</title>");
                var tag = match.Value;
                title = tag.Replace("<title>", "").Replace("</title>", "");
            }

            return title;
        }

        public string GetContent(string html)
        {
            var text = "";
            html = RemoveLinebreaks(html);

            if(html.Contains("<body"))
            {
                var match = Regex.Match(html, @"<body(.*?)>(.*?)</body>");          //Gets entire body of html
                var body = match.Value;
                body = ReplaceRegex(body, @"<style>(.*?)</style>", " ");            //Removes styles and it's contents
                body = ReplaceRegex(body, @"<script(.*?)>(.*?)</script>", " ");     //Removes scripts and it's contents
                body = ReplaceRegex(body, @"<(.*?)>", " ");                         //Removes all tags
                body = ReplaceRegex(body, @"&(.*?);", " ");                         //Removes all symbol entities
                body = ReplaceRegex(body, @"\s{2,}", " ");                          //Removes additional whitespaces
                body = ReplaceRegex(body, @"\s{2,}", " ");                          //Removes additional whitespaces
                text = body;
            }

            text = text.TrimStart(' ');
            text = text.TrimEnd(' ');
            return text;
        }

        public string RemoveLinebreaks(string html)
        {
            html = html.Replace("\n", " ");
            html = html.Replace("\r", " ");
            return html;
        }

        public string ConvertToUTF8(string html, Encoding encoding)
        {
            byte[] input = encoding.GetBytes(html);
            byte[] output = Encoding.Convert(encoding, Encoding.UTF8, input);
            return Encoding.UTF8.GetString(output);
        }

        public string TagsToLower(string html)
        {
            var matches = Regex.Matches(html, @"<(.*?)>");
            for (int i = 0; i < matches.Count; i++)
            {
                var tag = matches[i].Value.ToLower();
                html = html.Replace(matches[i].Value, tag);
            }
            return html;
        }

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
