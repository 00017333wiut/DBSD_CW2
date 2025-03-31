namespace CW2.DAL.Entities
{
    public interface IStaffRepository
    {
        IList<Staff>GetAll();
        Staff GetById(int id);
        Staff Insert(Staff staff);
        void Update(Staff staff); 
        void Delete(Staff staff);
    }
}
