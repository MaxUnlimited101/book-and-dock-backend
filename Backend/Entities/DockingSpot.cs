using System;

namespace Backend.Entities
{
    public class DockingSpot : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public User Owner { get; set; }
        public Port Port { get; set; }
        public double PricePerNight { get; set; }
        public double PricePerPerson { get; set; }
        public bool IsAvailable { get; set; } = false;

        public DockingSpot() { }
    }
}