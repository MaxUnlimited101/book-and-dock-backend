using System;

namespace Backend.Entities
{
    public class Location : BaseEntity
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Town { get; set; }
        public Port Port { get; set; }
        public DockingSpot DockingSpot { get; set; }

        public Location() { }
    }
}