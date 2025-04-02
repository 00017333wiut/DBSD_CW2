using System;
using System.Collections.Generic;

namespace CW2.DAL.Entities;

public partial class Location
{
    public int LocationId { get; set; }

    public string LocationName { get; set; } = null!;

    public string? Address { get; set; }

    public string? ContactNumber { get; set; }
}
