using System;
using System.Collections.Generic;

namespace CW2.DAL.Entities;

public partial class RentalRecord
{
    public int RentalId { get; set; }

    public DateOnly RentalDate { get; set; }

    public DateOnly? ReturnDate { get; set; }

    public decimal? Fine { get; set; }

    public int CustomerId { get; set; }

    public int ArtworkId { get; set; }

    public int StaffId { get; set; }

    public int? PaymentId { get; set; }

    public virtual Artwork Artwork { get; set; } = null!;

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual Staff Staff { get; set; } = null!;
}
