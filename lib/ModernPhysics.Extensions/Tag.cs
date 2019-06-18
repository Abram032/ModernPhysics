using ModernPhysics.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModernPhysics.Extensions
{
    public static class Tags
    {
        public static IEnumerable<Tag> ToTags(this string[] tags)
        {
            var _tags = new List<Tag>();

            foreach (var tag in tags)
            {
                _tags.Add(new Tag
                {
                    Name = tag
                });
            }

            return _tags;
        }

        public static string ToTagsString(this string[] tags)
        {
            var sb = new StringBuilder();
            foreach (var item in tags)
            {
                sb.Append(item);
                sb.Append(' ');
            }
            return sb.ToString();
        }
    }
}
