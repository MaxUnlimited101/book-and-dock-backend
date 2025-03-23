using System;
using System.Collections.Generic;

namespace Backend.Models;

public partial class DockingSpot
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public int OwnerId { get; set; }

    public int PortId { get; set; }

    public double? PricePerNight { get; set; }

    public double? PricePerPerson { get; set; }

    public bool IsAvailable { get; set; }

    public DateTime? CreatedOn { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<Location> Locations { get; set; } = new List<Location>();

    public virtual User Owner { get; set; } = null!;

    public virtual Port Port { get; set; } = null!;

    public virtual ICollection<Service> Services { get; set; } = new List<Service>();
}
