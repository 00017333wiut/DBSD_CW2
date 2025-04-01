using CW2.DAL.Entities;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace CW2.DAL.Repositories
{
    public class AdoNetStaffRepository : IStaffRepository
    {
        private const string SELECT_ALL_SQL = @"
                                SELECT StaffId, Name, Role, Contact
                                FROM Staff";

        private const string SELECT_BY_ID = @"
                                SELECT StaffId, Name, Role, Contact
                                FROM Staff
                                WHERE StaffId = @StaffId";

        private const string INSERT_SQL = @"
                                INSERT INTO Staff (Name, Role, Contact)
                                VALUES (@Name, @Role, @Contact);
                                SELECT SCOPE_IDENTITY();";

        private const string UPDATE_SQL = @"
                                UPDATE Staff
                                SET 
                                    Name  = @Name,
                                    Role  = @Role,
                                    Contact = @Contact
                                WHERE StaffId = @StaffId";

        private const string DELETE_SQL = @"
                                DELETE FROM Staff 
                                WHERE StaffId = @StaffId";


        private readonly string _connStr;

        public AdoNetStaffRepository(string connStr)
        {
            _connStr = connStr;
        }


        public void Delete(Staff staff)
        {
            using var conn = new SqlConnection(_connStr);
            using var cmd = conn.CreateCommand();
            cmd.CommandText = DELETE_SQL;
            cmd.Parameters.AddWithValue("StaffId", staff.StaffId);

            conn.Open();

            cmd.ExecuteNonQuery();
        }

        public IList<Staff> Filter(string? name, string? role, string? contact, int page = 1, int pageSize = 3, string sortColumn = "StaffId", bool sortDesc = false)
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
                var staff = MapReader(rdr);

                list.Add(staff);
            }
            return list;
        }

        public Staff GetById(int id)
        {
            using var conn = new SqlConnection(_connStr);
            var cmd = conn.CreateCommand();
            cmd.CommandText = SELECT_BY_ID;

            var pStaffId = cmd.CreateParameter();
            pStaffId.ParameterName = "StaffID";
            pStaffId.Value = id;
            pStaffId.DbType = DbType.Int32;
            pStaffId.Direction = ParameterDirection.Input;
            cmd.Parameters.Add(pStaffId);

            conn.Open();

            using var rdr = cmd.ExecuteReader();
            Staff? staff = null;
            if (rdr.Read())
            {
                staff = MapReader(rdr);
            }
            return staff;

        }

        public Staff Insert(Staff staff)
        {
            using var conn = new SqlConnection(_connStr);
            using var cmd = conn.CreateCommand();
            cmd.CommandText = INSERT_SQL;
            cmd.Parameters.AddWithValue("Name", staff.Name);
            cmd.Parameters.AddWithValue("Role", staff.Role);
            cmd.Parameters.AddWithValue("Contact", staff.Contact ?? (object)DBNull.Value);

            conn.Open();
            int id = Convert.ToInt32(cmd.ExecuteScalar());
            staff.StaffId = id;

            return staff;
        }

        public void Update(Staff staff)
        {
            using var conn = new SqlConnection(_connStr);
            using var cmd = conn.CreateCommand();
            cmd.CommandText = UPDATE_SQL;
            cmd.Parameters.AddWithValue("Name", staff.Name);
            cmd.Parameters.AddWithValue("Role", staff.Role);
            cmd.Parameters.AddWithValue("Contact", staff.Contact ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("StaffId", staff.StaffId);

            conn.Open();

            cmd.ExecuteNonQuery();
        }


        private Staff MapReader(DbDataReader rdr)
        {
            return new Staff()
            {
                StaffId = rdr.GetInt32("StaffID"),
                Name = rdr.GetString("Name"),
                Role = rdr.GetString("Role"),
                Contact = rdr.GetString("Contact")
            };
        }
    }

}
