using System;

namespace Backend.Models;

public partial class Review
{
    public int Id { get; set; }

    // Renamed from CreatedBy to UserId.
    public int UserId { get; set; }

    public double Rating { get; set; }

    // Renamed from Comment to Content.
    public string Content { get; set; } = null!;

    // Renamed from CreatedOn to CreatedAt.
    public DateTime CreatedAt { get; set; }

    // Added UpdatedAt.
    public DateTime? UpdatedAt { get; set; }

    // Renamed from PortId to DockId to match DTO naming.
    public int DockId { get; set; }

    // Updated navigation properties accordingly.
    public virtual User User { get; set; } = null!;

    public virtual Port Dock { get; set; } = null!;
}