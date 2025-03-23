using System;
using System.Collections.Generic;

namespace Backend.Models;

public partial class Image
{
    public int Id { get; set; }

    public string Url { get; set; } = null!;

    public int? CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public int? GuideId { get; set; }

    public virtual User? CreatedByNavigation { get; set; }

    public virtual Guide? Guide { get; set; }
}
