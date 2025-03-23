using System;
using System.Collections.Generic;

namespace Backend.Models;

public partial class Guide
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public int? CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public bool IsApproved { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual User? CreatedByNavigation { get; set; }

    public virtual ICollection<Image> Images { get; set; } = new List<Image>();
}
