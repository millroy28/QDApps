using System;
using System.Collections.Generic;

namespace QDApps.Models;

public partial class User
{
    public int UserId { get; set; }

    public string AspNetUserId { get; set; } = null!;

    public string? UserName { get; set; }

    public int TimeZoneId { get; set; }

    public virtual AspNetUser AspNetUser { get; set; } = null!;

    public virtual ICollection<Stash> Stashes { get; set; } = new List<Stash>();

    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();

    public virtual TimeZone TimeZone { get; set; } = null!;
}
