using System;

namespace Backend.Entities
{
    public class Guide : BaseEntity
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public User User { get; set; }
        public bool IsApproved { get; set; } = false;

        public Guide() { }
    }
}