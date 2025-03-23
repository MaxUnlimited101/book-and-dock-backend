using System;

namespace Backend.Entities
{
    public class Notification : BaseEntity
    {
        public User CreatedBy { get; set; }
        public string Message { get; set; }

        public Notification() { }
    }
}