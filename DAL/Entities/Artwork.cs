using System.ComponentModel.DataAnnotations;

namespace CW2.DAL.Entities
{
    public class Artwork
    {
        
        public int ArtworkId { get; set; }
        public required string Title { get; set; }
        public int? ArtistId { get; set; }
        public int? CategoryId { get; set; }

        public int? Year { get; set; }

        public required decimal RentalPrice { get; set; }

        public DateTime? Availability { get; set; }

        public bool? IsAvailable { get; set; } = true;

        public byte[]? ArtworkImage { get; set; }
    }
}
