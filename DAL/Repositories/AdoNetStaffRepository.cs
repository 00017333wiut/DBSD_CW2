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
            throw new NotImplementedException();
        }

        public void Update(Staff staff)
        {
            throw new NotImplementedException();
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
