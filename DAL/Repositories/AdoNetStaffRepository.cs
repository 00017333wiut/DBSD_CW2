using CW2.DAL.Entities;
using System.Data;
using System.Data.SqlClient;

namespace CW2.DAL.Repositories
{
    public class AdoNetStaffRepository : IStaffRepository
    {
        private const string SELECT_ALL_SQL = @"
                                select StaffId, Name, Role, Contact
                                from Staff";

        private readonly string _connStr;

        public AdoNetStaffRepository(string connStr)
        {
            _connStr = connStr;
        }


        public void Delete(Staff staff)
        {
            throw new NotImplementedException();
        }

        public IList<Staff> GetAll()
        {
            var list = new List<Staff>();

            using var conn = new SqlConnection(_connStr);
            var cmd = conn.CreateCommand();
            cmd.CommandText = SELECT_ALL_SQL;

            conn.Open();
            using var rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                var staff = new Staff()
                {
                    StaffId = rdr.GetInt32("StaffID"),
                    Name = rdr.GetString("Name"),
                    Role = rdr.GetString("Role"),
                    Contact = rdr.GetString("Contact")
                };

                list.Add(staff);
            }
            return list;
        }

        public Staff GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Staff Insert(Staff staff)
        {
            throw new NotImplementedException();
        }

        public void Update(Staff staff)
        {
            throw new NotImplementedException();
        }
    }
}
