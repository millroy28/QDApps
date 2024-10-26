using System;
using System.Collections.Generic;

namespace QDApps.Models.WhereItAppModels;

public partial class ItemTag
{
    public int ItemTagId { get; set; }

    public int TagId { get; set; }

    public int ItemId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Item Item { get; set; } = null!;

    public virtual Tag Tag { get; set; } = null!;
}
