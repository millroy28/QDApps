using System;
using System.Collections.Generic;

namespace QDApps.Models;

public partial class TimeZone
{
    public int TimeZoneId { get; set; }

    public string TimeZoneName { get; set; } = null!;

    public int Utcoffset { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
