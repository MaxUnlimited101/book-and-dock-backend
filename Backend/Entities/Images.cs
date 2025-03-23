using System;

namespace Backend.Entities
{
    public class Image : BaseEntity
    {
        public string Url { get; set; }
        public User CreatedBy { get; set; }
        public Guide Guide { get; set; }

        public Image() { }
    }
}