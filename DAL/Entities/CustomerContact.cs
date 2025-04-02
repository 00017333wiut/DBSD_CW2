using System;
using System.Collections.Generic;

namespace CW2.DAL.Entities;

public partial class CustomerContact
{
    public int CustomerId { get; set; }

    public string? Contact { get; set; }

    public virtual Customer Customer { get; set; } = null!;
}
