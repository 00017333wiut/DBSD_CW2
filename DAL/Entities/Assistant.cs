using System;
using System.Collections.Generic;

namespace CW2.DAL.Entities;

public partial class Assistant
{
    public int StaffId { get; set; }

    public int AssignedTo { get; set; }

    public string? ShiftSchedule { get; set; }

    public string? TrainingLevel { get; set; }

    public virtual Staff Staff { get; set; } = null!;
}
