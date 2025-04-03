using CW2.DAL.Entities;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace CW2.DAL.Repositories
{
    public class AdoNetArtworkRepository : IArtworkRepository
    {
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


        private readonly string _connStr;

        public AdoNetArtworkRepository(string connStr)
        {
            _connStr = connStr;
        }


        public void Delete(Artwork artwork)
        {
            using var conn = new SqlConnection(_connStr);
            using var cmd = conn.CreateCommand();
            cmd.CommandText = DELETE_SQL;
            cmd.Parameters.AddWithValue("ArtworkId", artwork.ArtworkId);

            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public IList<Artwork> Filter(string? title, DateTime? availability, int artistId, int page = 1, int pageSize = 10, string sortColumn = "ArtworkID", bool sortDesc = false)
        {
            throw new NotImplementedException();
        }

        public IList<Artwork> GetAll()
        {
            var list = new List<Artwork>();

            using var conn = new SqlConnection(_connStr);
            var cmd = conn.CreateCommand();
            cmd.CommandText = SELECT_ALL_SQL;

            conn.Open();
            using var rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                var artwork = MapReader(rdr);

                list.Add(artwork);
            }
            return list;
        }

        public Artwork GetById(int id)
        {
            using var conn = new SqlConnection(_connStr);
            var cmd = conn.CreateCommand();
            cmd.CommandText = SELECT_BY_ID;

            var pArtworkId = cmd.CreateParameter();
            pArtworkId.ParameterName = "ArtworkID";
            pArtworkId.Value = id;
            pArtworkId.DbType = DbType.Int32;
            pArtworkId.Direction = ParameterDirection.Input;
            cmd.Parameters.Add(pArtworkId);

            conn.Open();

            using var rdr = cmd.ExecuteReader();
            Artwork? artwork = null;
            if (rdr.Read())
            {
                artwork = MapReader(rdr);
            }
            return artwork;

        }

        public Artwork Insert(Artwork artwork)
        {
            using var conn = new SqlConnection(_connStr);
            using var cmd = conn.CreateCommand();
            cmd.CommandText = INSERT_SQL;

            cmd.Parameters.AddWithValue("Title", artwork.Title);
            cmd.Parameters.AddWithValue("ArtistId", artwork.ArtistId);
            cmd.Parameters.AddWithValue("CategoryID", artwork.CategoryId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("Year", artwork.Year ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("RentalPrice", artwork.RentalPrice);
            cmd.Parameters.AddWithValue("Availability", artwork.Availability ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("IsAvailable", artwork.IsAvailable);
            cmd.Parameters.AddWithValue("ArtworkImage", artwork.ArtworkImage ?? (object)DBNull.Value);

            conn.Open();
            int id = Convert.ToInt32(cmd.ExecuteScalar());
            artwork.ArtworkId = id;

            return artwork;
        }

        public void Update(Artwork artwork)
        {
            using var conn = new SqlConnection(_connStr);
            using var cmd = conn.CreateCommand();
            cmd.CommandText = UPDATE_SQL;

            cmd.Parameters.AddWithValue("Title", artwork.Title);
            cmd.Parameters.AddWithValue("ArtistID", artwork.ArtistId);
            cmd.Parameters.AddWithValue("CategoryID", artwork.CategoryId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("Year", artwork.Year ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("RentalPrice", artwork.RentalPrice);
            cmd.Parameters.AddWithValue("Availability", artwork.Availability ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("IsAvailable", artwork.IsAvailable);
            cmd.Parameters.AddWithValue("ArtworkImage", artwork.ArtworkImage ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("ArtworkID", artwork.ArtworkId);


            conn.Open();
            cmd.ExecuteNonQuery();
        }


        private Artwork MapReader(DbDataReader rdr)
        {
            return new Artwork()
            {
                ArtworkId = rdr.GetInt32("ArtworkID"), 
                Title = rdr.GetString("Title"), 
                ArtistId = rdr.IsDBNull("ArtistID") ? null : rdr.GetInt32("ArtistID"),
                CategoryId = rdr.IsDBNull("CategoryID") ? null : rdr.GetInt32("CategoryID"), 
                Year = rdr.IsDBNull("Year") ? null : rdr.GetInt32("Year"), 
                RentalPrice = rdr.GetDecimal("RentalPrice"), 
                Availability = rdr.IsDBNull("Availability") ? null : rdr.GetDateTime("Availability"),
                IsAvailable = rdr.GetBoolean("IsAvailable"), 
                ArtworkImage = rdr.IsDBNull("ArtworkImage") ? null : (byte[])rdr["ArtworkImage"] 

            };
        }
    }

}
