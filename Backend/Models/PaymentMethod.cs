using System;
using System.Collections.Generic;

namespace Backend.Models;

public partial class PaymentMethod
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime? CreatedOn { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
