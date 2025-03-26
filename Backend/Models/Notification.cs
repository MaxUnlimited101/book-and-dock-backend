using System;
using System.Collections.Generic;

namespace Backend.Models;

public partial class Notification
{
    public int Id { get; set; }

    public int? CreatedBy { get; set; }

    public string Message { get; set; } = null!;

    public DateTime? CreatedOn { get; set; }

    public virtual User? CreatedByNavigation { get; set; }
}
