using System;
using System.Collections.Generic;

namespace QDApps.Models.WhereItAppModels;

public partial class Item
{
    public int ItemId { get; set; }

    public int? StashId { get; set; }
    public string ItemName { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<ItemTag> ItemTags { get; set; } = new List<ItemTag>();

    public virtual Stash? Stash { get; set; }
}
