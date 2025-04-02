using System;
using System.Collections.Generic;

namespace CW2.DAL.Entities;

public partial class Staff
{
    public int StaffId { get; set; }

    public string Name { get; set; } = null!;

    public string Role { get; set; } = null!;

    public string? Contact { get; set; }

    public virtual Assistant? Assistant { get; set; }

    public virtual Manager? Manager { get; set; }

    public virtual ICollection<RentalRecord> RentalRecords { get; set; } = new List<RentalRecord>();
}
