using System;
using System.Collections.Generic;

namespace Backend.Models;

public partial class Comment
{
    public int Id { get; set; }

    /// <summary>
    /// The user id, who created the comment
    /// </summary>
    public int? CreatedBy { get; set; }

    public int? GuideId { get; set; }

    public string Content { get; set; } = null!;

    public DateTime? CreatedOn { get; set; }

    /// <summary>
    /// The user who created the comment
    /// </summary>
    public virtual User? CreatedByNavigation { get; set; }

    public virtual Guide? Guide { get; set; }
}
