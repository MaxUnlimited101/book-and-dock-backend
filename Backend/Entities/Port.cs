using System;

namespace Backend.Entities
{
    public class Port : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public User Owner { get; set; }
        public bool IsApproved { get; set; } = false;

        public Port() { }
    }
}