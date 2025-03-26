using System;
using System.Collections.Generic;

namespace Backend.Models;

public partial class Service
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public int? PortId { get; set; }

    public int? DockingSpotId { get; set; }

    public bool IsAvailable { get; set; }

    public DateTime? CreatedOn { get; set; }

    public virtual DockingSpot? DockingSpot { get; set; }

    public virtual Port? Port { get; set; }
}
