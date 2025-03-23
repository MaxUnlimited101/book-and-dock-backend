using System;

namespace Backend.Entities
{
    public class Review : BaseEntity
    {
        public User CreatedBy { get; set; }
        public double Rating { get; set; }
        public string Comment { get; set; }
        public Port Port { get; set; }

        public Review() { }
    }
}