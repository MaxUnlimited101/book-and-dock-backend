using System;
using System.Collections.Generic;

namespace Backend.Models;

public partial class Review
{
    public int Id { get; set; }

    public int? CreatedBy { get; set; }

    public double Rating { get; set; }

    public string? Comment { get; set; }

    public DateTime? CreatedOn { get; set; }

    public int PortId { get; set; }

    public virtual User? CreatedByNavigation { get; set; }

    public virtual Port Port { get; set; } = null!;
}
