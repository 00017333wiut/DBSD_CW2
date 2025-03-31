namespace CW2.DAL.Entities
{
    public class Staff
    {
        public int StaffId { get; set; }
        public required string Name { get; set; }
        public string? Role { get; set; }
        public string? Contact { get; set; }
    }
}
