using System;
using System.Collections.Generic;

namespace Backend.Models;

public partial class Comment
{
    public int Id { get; set; }

    public int? CreatedBy { get; set; }

    public int? GuideId { get; set; }

    public string Content { get; set; } = null!;

    public DateTime? CreatedOn { get; set; }

    public virtual User? CreatedByNavigation { get; set; }

    public virtual Guide? Guide { get; set; }
}
