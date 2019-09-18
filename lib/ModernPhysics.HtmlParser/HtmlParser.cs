using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ModernPhysics.HtmlParser
{
    public class HtmlParser
    {
        public string GetTitle(string html)
        {
            string title = "";

            if (html.Contains("<title>"))
            {
                var match = Regex.Match(html, "<title>(.*?)</title>");
                var tag = match.Value;
                title = tag.Replace("<title>", "").Replace("</title>", "");
                title = ReplaceRegex(title, @"&(.*?);", " ");
            }

            return title;
        }

        public string GetContent(string html)
        {
            html = ReplaceRegex(html, @"<!--(.*?)-->", " ");                    //Removes all comments
            html = ReplaceRegex(html, @"<title>(.*?)</title>", " ");            //Removes title tag
            html = ReplaceRegex(html, @"<style(.*?)>(.*?)</style>", " ");       //Removes styles and it's contents
            html = ReplaceRegex(html, @"<script(.*?)>(.*?)</script>", " ");     //Removes scripts and it's contents
            html = ReplaceRegex(html, @"<(.*?)>", " ");                         //Removes all other tags
            html = ReplaceRegex(html, @"&(.*?);", " ");                         //Removes all symbol entities
            html = ReplaceRegex(html, @"&nbsp", " ");                           //Removes incorrect &nbsp symbols
            html = ReplaceRegex(html, @"\t", " ");                              //Removes tabulaions
            html = ReplaceRegex(html, @"\s{2,}", " ");                          //Removes additional whitespaces
            html = ReplaceRegex(html, @"\s{2,}", " ");                          //Removes additional whitespaces
            html = ReplaceRegex(html, @"\s{2,}", " ");                          //Removes additional whitespaces

            html = html.TrimStart(' ');
            html = html.TrimEnd(' ');
            return html;
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
