using System;
using System.Collections.Generic;
using System.Text;

namespace ModernPhysics.Models
{
    public class Tag
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<PageTag> PageTags { get; set; }
    }
}
