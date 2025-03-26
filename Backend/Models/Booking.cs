using System;
using System.Collections.Generic;

namespace Backend.Models;

public partial class Booking
{
    public int Id { get; set; }

    public int SailorId { get; set; }

    public int DockingSpotId { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public int PaymentMethodId { get; set; }

    public bool IsPaid { get; set; }

    public int People { get; set; }

    public DateTime? CreatedOn { get; set; }

    public virtual DockingSpot DockingSpot { get; set; } = null!;

    public virtual PaymentMethod PaymentMethod { get; set; } = null!;

    public virtual User Sailor { get; set; } = null!;
}
