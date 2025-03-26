using System;
using System.Collections.Generic;

namespace Backend.Models;

public partial class Location
{
    public int Id { get; set; }

    public DateTime? CreatedOn { get; set; }

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public string Town { get; set; } = null!;

    public int? PortId { get; set; }

    public int? DockingSpotId { get; set; }

    public virtual DockingSpot? DockingSpot { get; set; }

    public virtual Port? Port { get; set; }
}
