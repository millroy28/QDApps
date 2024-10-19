using System;
using System.Collections.Generic;

namespace QDApps.Models;

public partial class Tag
{
    public int TagId { get; set; }

    public int UserId { get; set; }

    public string TagName { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<ItemTag> ItemTags { get; set; } = new List<ItemTag>();

    public virtual User User { get; set; } = null!;
}
