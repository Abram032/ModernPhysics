using System;
using System.Collections.Generic;

namespace ModernPhysics.Models
{
    public class Question
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public Quiz Quiz { get; set; }
        public ICollection<Answer> Answers { get; set; }
    }
}
