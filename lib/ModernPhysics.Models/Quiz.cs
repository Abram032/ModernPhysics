using System;
using System.Collections.Generic;

namespace ModernPhysics.Models
{
    public class Quiz
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string FriendlyUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedAt { get; set; }
        public string ModifiedBy { get; set; }
        public ICollection<Question> Questions { get; set; }
        public int TimesSolved { get; set; }
        public int TimesSolvedCorrectly { get; set; }
        public bool IsPublished { get; set; }
        public bool IsDeleted { get; set; }

        public Guid? PostId { get; set; }
        public Post Post { get; set; }
    }
}
