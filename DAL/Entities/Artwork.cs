using System;
using System.Collections.Generic;

namespace CW2.DAL.Entities;

public partial class Artwork
{
    public int ArtworkId { get; set; }

    public string Title { get; set; } = null!;

    public int? ArtistId { get; set; }

    public int? CategoryId { get; set; }

    public int? Year { get; set; }

    public decimal? RentalPrice { get; set; }

    public DateTime? Availability { get; set; }

    public bool? IsAvailable { get; set; }

    public byte[]? ArtworkImage { get; set; }

    public virtual Artist? Artist { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<RentalRecord> RentalRecords { get; set; } = new List<RentalRecord>();
}
