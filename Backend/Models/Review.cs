using System;
using System.Collections.Generic;

namespace Backend.Models;

public partial class Review
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public double Rating { get; set; }

    public string Content { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int DockId { get; set; }

    public virtual Port Dock { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
