using System;
using System.Collections.Generic;
using System.Text;

namespace ModernPhysics.Models
{
    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        //Font Awesome 5 Icon Name
        public string FriendlyName { get; set; }
        public string Icon { get; set; }
        public ICollection<Page> Pages { get; set; }
    }
}
