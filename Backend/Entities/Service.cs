using System;

namespace Backend.Entities
{
    public class Service : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public Port Port { get; set; }
        public DockingSpot DockingSpot { get; set; }
        public bool IsAvailable { get; set; } = true;

        public Service() { }
    }
}