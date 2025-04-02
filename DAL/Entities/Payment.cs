using System;
using System.Collections.Generic;

namespace CW2.DAL.Entities;

public partial class Payment
{
    public int PaymentId { get; set; }

    public DateOnly PaymentDate { get; set; }

    public decimal Amount { get; set; }

    public string PaymentMethod { get; set; } = null!;

    public int RentalId { get; set; }

    public virtual RentalRecord Rental { get; set; } = null!;
}
