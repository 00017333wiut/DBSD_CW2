namespace CW2.DAL.Entities
{
    public interface IStaffRepository
    {
        IList<Staff> GetAll();
        Staff? GetById(int id);
        Staff Insert(Staff staff);
        void Update(Staff staff);
        void Delete(Staff staff);
        IList<Staff> Filter(string? name,
                            string? role,
                            string? contact,
                            int page = 1,
                            int pageSize = 3,
                            string sortColumn = "StaffId",
                            bool sortDesc = false);
    }
}
