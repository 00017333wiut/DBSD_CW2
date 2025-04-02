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
        private const string FILTER_SQL = @"
                            select ArtworkID, Title, ArtistID, CategoryID, Year, RentalPrice, Availability, IsAvailable
                            from Artwork
                            where Title like concat(@Title, '%')
                              and Availability >= coalesce(@Availability, '1900-01-01')
                              and (@ArtistID = 0 OR ArtistID = @ArtistID) 
                            order by {0}
                            offset (@Page-1)*@PageSize rows fetch next @PageSize rows only";

        #endregion sql constants

        private readonly string _connStr;
        private readonly IList<string> ALLOWED_SORT_COLUMNS = new List<string>() { "ArtworkId", "Title" };

        public DapperArtworkRepository(string connStr)
        {
            _connStr = connStr;
        }

        public void Delete(Artwork employee)
        {
            using var conn = new SqlConnection(_connStr);
            conn.Execute(DELETE_SQL, new { employee.ArtworkId });
        }
        public IList<Artwork> Filter(string? title, DateTime? availability, int artistId, int page = 1, int pageSize = 10, string sortColumn = "ArtworkID", bool sortDesc = false)
        {


            page = page > 0 ? page : 1;  
            pageSize = pageSize > 0 ? pageSize : 10;

            if (!ALLOWED_SORT_COLUMNS.Contains(sortColumn))
            {
                sortColumn = "ArtworkId";
            }

            string sql = string.Format(FILTER_SQL, sortColumn + (sortDesc ? " DESC " : " ASC "));

            using var conn = new SqlConnection(_connStr);
            return conn.Query<Artwork>(sql,
                new
                {
                    ArtistID = artistId,
                    Title = title,
                    Availability = availability as DateTime?, 
                    Page = page,
                    PageSize = pageSize,
                    //SortColumn = sortColumn,

                }
                ).ToList();
        }

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

