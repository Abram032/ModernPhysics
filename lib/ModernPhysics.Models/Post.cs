using System;
using System.Collections.Generic;

namespace ModernPhysics.Models
{
    public class Post
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string FriendlyUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedAt { get; set; }
        public string ModifiedBy { get; set; }
        public Category Category { get; set; }
        public ContentType ContentType { get; set; }
        public string Shortcut { get; set; }
        public string Content { get; set; }
        public bool IsPublished { get; set; }
        public bool IsDeleted { get; set; }
    }

    public enum ContentType 
    {
        Html, Markdown, CKEditor, Text  
    }
}
