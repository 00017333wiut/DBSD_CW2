namespace CW2.Models
{
    public class ArtworkViewModel
    {
        public int ArtworkId { get; set; }
        public required string Title { get; set; }
        public int ArtistId { get; set; }
        public int CategoryId { get; set; }
        public int? Year { get; set; }
        public decimal RentalPrice { get; set; }
        public DateTime? Availability { get; set; }
        public bool IsAvailable { get; set; }
        public byte[]? ArtworkImage { get; set; }
        
    }

}

