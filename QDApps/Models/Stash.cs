using System;
using System.Collections.Generic;

namespace QDApps.Models;

public partial class Stash
{
    public int StashId { get; set; }

    public int UserId { get; set; }

    public string StashName { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Item> Items { get; set; } = new List<Item>();

    public virtual User User { get; set; } = null!;
}
