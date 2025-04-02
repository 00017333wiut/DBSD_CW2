using System;
using System.Collections.Generic;

namespace CW2.DAL.Entities;

public partial class Promotion
{
    public int PromotionId { get; set; }

    public decimal? DiscountPercentage { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public int CustomerId { get; set; }

    public virtual Customer Customer { get; set; } = null!;
}
