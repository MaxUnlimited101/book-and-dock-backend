using System;
using System.Collections.Generic;

namespace Backend.Models;

public partial class Port
{
    public int Id { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int OwnerId { get; set; }

    public bool IsApproved { get; set; }

    public virtual ICollection<DockingSpot> DockingSpots { get; set; } = new List<DockingSpot>();

    public virtual ICollection<Location> Locations { get; set; } = new List<Location>();

    public virtual User Owner { get; set; } = null!;

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual ICollection<Service> Services { get; set; } = new List<Service>();
}
