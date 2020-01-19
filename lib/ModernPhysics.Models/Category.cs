using System;
using System.Collections.Generic;
using System.Text;

namespace ModernPhysics.Models
{
    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string FriendlyName { get; set; }
        public string Icon { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedAt { get; set; }
        public string ModifiedBy { get; set; }
        public ICollection<Post> Posts { get; set; }
    }
}
