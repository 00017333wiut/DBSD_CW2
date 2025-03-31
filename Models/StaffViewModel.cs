namespace CW2.Models
{
    public class StaffViewModel
    {
        public int StaffId { get; set; }
        public required string Name { get; set; }
        public string? Role { get; set; }
        public string? Contact { get; set; }
    }
}
