using System;

namespace Backend.Entities
{
    public class Comment : BaseEntity
    {
        public User CreatedBy { get; set; }
        public Guide Guide { get; set; }
        public string Content { get; set; }

        public Comment() { }
    }
}