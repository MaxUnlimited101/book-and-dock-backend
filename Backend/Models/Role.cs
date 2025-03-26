using System;
using System.Collections.Generic;

namespace Backend.Models;

public partial class Role
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime? CreatedOn { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
