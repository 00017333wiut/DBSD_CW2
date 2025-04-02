using System;
using System.Collections.Generic;

namespace CW2.DAL.Entities;

public partial class Manager
{
    public int StaffId { get; set; }

    public int? ManagedTeams { get; set; }

    public decimal? AnnualBonus { get; set; }

    public string? AuthorityLevel { get; set; }

    public virtual Staff Staff { get; set; } = null!;
}
