using ModernPhysics.Models;
using System;
using System.Collections.Generic;

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
    }
}
