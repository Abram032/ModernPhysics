using System;
using System.Collections.Generic;

namespace ModernPhysics.Models
{
    public class Page
    {
        int Id { get; set; }
        Guid Guid { get; set; }
        string Title { get; set; }
        DateTime CreatedAt { get; set; }
        string CreatedBy { get; set; }
        DateTime ModifiedAt { get; set; }
        string ModifiedBy { get; set; }
        ICollection<string> Tags { get; set; }
        string Content { get; set; }
        bool IsPublished { get; set; }
        bool IsDeleted { get; set; }
    }
}
