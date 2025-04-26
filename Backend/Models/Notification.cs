using System;
using System.Collections.Generic;

namespace Backend.Models;

public partial class Notification
{
    public int Id { get; set; }

    /// <summary>
    /// The user id, who created the notification
    /// </summary>
    public int? CreatedBy { get; set; }

    public string Message { get; set; } = null!;

    public DateTime? CreatedOn { get; set; }

    /// <summary>
    /// The user who created the notification
    /// </summary>
    public virtual User? CreatedByNavigation { get; set; }
}
