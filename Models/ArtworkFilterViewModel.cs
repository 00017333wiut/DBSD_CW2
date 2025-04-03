using CW2.DAL.Entities;

namespace CW2.Models
{
    public class ArtworkFilterViewModel
    {
        public string? Title { get; set; }
        public DateTime? Availability { get; set; }
        public int ArtistId { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string SortColumn { get; set; } = "EmployeeID";
        public bool SortDesc { get; set; } = false;
        public IEnumerable<ArtworkViewModel> Artwork { get; set; }
    }
}
