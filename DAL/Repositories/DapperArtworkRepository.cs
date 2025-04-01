using CW2.DAL.Entities;
using Dapper;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
namespace CW2.DAL.Repositories
{
    public class DapperArtworkRepository :IArtworkRepository
    {
        #region sql constants
        private const string SELECT_ALL_SQL = @"
                        SELECT ArtworkID, Title, ArtistID, CategoryID, Year, RentalPrice, Availability, IsAvailable, ArtworkImage
                        FROM Artwork";

        private const string SELECT_BY_ID = @"
                        SELECT ArtworkID, Title, ArtistId, CategoryID, Year, RentalPrice, Availability, IsAvailable, ArtworkImage
                        FROM Artwork
                        WHERE ArtworkID = @ArtworkID";

        private const string INSERT_SQL = @"
                        INSERT INTO Artwork (Title, ArtistId, CategoryID, Year, RentalPrice, Availability, IsAvailable, ArtworkImage)
                        VALUES (@Title, @ArtistId, @CategoryID, @Year, @RentalPrice, @Availability, @IsAvailable, @ArtworkImage);
                        SELECT SCOPE_IDENTITY();";

        private const string UPDATE_SQL = @"
                        UPDATE Artwork
                        SET
                            Title = @Title,
                            ArtistID = @ArtistID,
                            CategoryID = @CategoryID,
                            Year = @Year,
                            RentalPrice = @RentalPrice,
                            Availability = @Availability,
                            IsAvailable = @IsAvailable,
                            ArtworkImage = @ArtworkImage
                        WHERE ArtworkID = @ArtworkID;";

        private const string DELETE_SQL = @"
                            DELETE FROM Artwork
                            WHERE ArtworkID = @ArtworkID;";
        #endregion sql constants

        private readonly string _connStr;
        private readonly IList<string> ALLOWED_SORT_COLUMNS = new List<string>() { "ArtworkId", "FirstName" };

        public DapperArtworkRepository(string connStr)
        {
            _connStr = connStr;
        }

        public void Delete(Artwork employee)
        {
            using var conn = new SqlConnection(_connStr);
            conn.Execute(DELETE_SQL, new { employee.ArtworkId });
        }

        public string ExportToJson(string? firstName, DateTime? birthDate, string sortColumn = "ArtworkId", bool sortDesc = false)
        {
            throw new NotImplementedException();
        }

        public string ExportToXml(string? firstName, DateTime? birthDate, string sortColumn = "ArtworkId", bool sortDesc = false)
        {
            throw new NotImplementedException();
        }

        public IList<Artwork> Filter(string? title, string? artistId, int? categoryId, int? year, decimal? minRentalPrice, decimal? maxRentalPrice, bool? isAvailable, int page = 1, int pageSize = 10, string sortColumn = "ArtworkID", bool sortDesc = false)
        {
            throw new NotImplementedException();
        }

        //public IList<Artwork> Filter(string? firstName,
        //                              DateTime? birthDate,
        //                              int[]? reportsToIds,
        //                              int page = 1,
        //                              int pageSize = 3,
        //                              string sortColumn = "ArtworkId",
        //                              bool sortDesc = false)
        //{
        //    if (page < 1)
        //    {
        //        page = 1;
        //    }

        //    if (reportsToIds == null)
        //    {
        //        reportsToIds = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        //    }

        //    if (!ALLOWED_SORT_COLUMNS.Contains(sortColumn))
        //    {
        //        sortColumn = "ArtworkId";
        //    }

        //    string sql = string.Format(FILTER_SQL, sortColumn + (sortDesc ? " DESC " : " ASC "));

        //    using var conn = new SqlConnection(_connStr);
        //    return conn.Query<Artwork>(sql,
        //                                new
        //                                {
        //                                    FirstName = firstName,
        //                                    BirthDate = birthDate,
        //                                    ReportsToIds = reportsToIds,
        //                                    Page = page,
        //                                    PageSize = pageSize,
        //                                }
        //        ).ToList();
        //}

        public IList<Artwork> GetAll()
        {
            using var conn = new SqlConnection(_connStr);
            return conn.Query<Artwork>(SELECT_ALL_SQL).ToList();
        }

        public Artwork? GetById(int id)
        {
            using var conn = new SqlConnection(_connStr);
            return conn.QueryFirstOrDefault<Artwork>(SELECT_BY_ID,
                                               new { ArtworkId = id });
        }

        public Artwork Insert(Artwork employee)
        {
            using var conn = new SqlConnection(_connStr);
            employee.ArtworkId = conn.ExecuteScalar<int>(INSERT_SQL, employee);

            return employee;
        }

        public void Update(Artwork employee)
        {
            using var conn = new SqlConnection(_connStr);
            conn.Execute(UPDATE_SQL, employee);
        }
    }
}

