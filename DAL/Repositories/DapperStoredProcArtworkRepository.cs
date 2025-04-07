using CW2.DAL.Entities;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace CW2.DAL.Repositories
{
    public class DapperStoredProcArtworkRepository : IArtworkRepository
    {
        private readonly IList<string> ALLOWED_SORT_COLUMNS = new List<string>() { "ArtworkId", "Title" };

        private readonly string _connStr;

        public DapperStoredProcArtworkRepository(string connStr)
        {
            _connStr = connStr;
        }

        public void Delete(Artwork artwork)
        {
            throw new NotImplementedException();
        }

        public IList<Artwork> EfFilter(string? title, string? artistId, int? categoryId, int? year, decimal? minRentalPrice, decimal? maxRentalPrice, bool? isAvailable, int page = 1, int pageSize = 10, string sortColumn = "ArtworkID", bool sortDesc = false)
        {
            throw new NotImplementedException();
        }

        public string ExportToXml(string? title = null,
                         int? year = null,
                         string sortColumn = "ArtworkID",
                         bool sortDesc = false)
        {
            using (var conn = new SqlConnection(_connStr))
            {
                var p = new DynamicParameters();
                p.Add("@Title", title);
                p.Add("@Year", year);
                p.Add("@SortColumn", sortColumn);
                p.Add("@SortDesc", sortDesc);
                p.Add("@XmlData", dbType: DbType.Xml, direction: ParameterDirection.Output);

                conn.Execute(
                    "udpArtworkExportAsXml",
                    param: p,
                    commandType: CommandType.StoredProcedure);

                return p.Get<string>("@XmlData");
            }
        }

        public string ExportToJson(
    string? title = null,
    int? year = null,
    string sortColumn = "ArtworkID",
    bool sortDesc = false)
        {
            using (var conn = new SqlConnection(_connStr))
            {
                conn.Open();
                var p = new DynamicParameters();
                p.Add("@Title", title);
                p.Add("@Year", year);
                p.Add("@SortColumn", sortColumn);
                p.Add("@SortDesc", sortDesc);
                p.Add("@JsonData", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);

                conn.Execute(
                    "udpArtworkExportAsJson",
                    p,
                    commandType: CommandType.StoredProcedure);

                return p.Get<string>("@JsonData");
            }
        }

        //
        public int ImportFromJson(string jsonData)
        {
            using (var conn = new SqlConnection(_connStr))
            {
                conn.Open();
                var p = new DynamicParameters();
                p.Add("@JsonData", jsonData);

                
                return conn.ExecuteScalar<int>(
                    "udpArtworkImportFromJson",
                    p,
                    commandType: CommandType.StoredProcedure);
            }
        }

        public IList<Artwork> Filter(string? title,
                                     DateTime? availability,
                                     int artistId,
                                     int page = 1,
                                     int pageSize = 10,
                                     string sortColumn = "ArtworkID",
                                     bool sortDesc = false)
        {
            throw new NotImplementedException();
        }

        public IList<Artwork> GetAll()
        {
            using var conn = new SqlConnection(_connStr);
            return conn.Query<Artwork>("dbo.udpGetAllArtworks",
                                        commandType: System.Data.CommandType.StoredProcedure
                ).ToList();
        }

        public Artwork? GetById(int id)
        {
            using var conn = new SqlConnection(_connStr);
            return conn.QueryFirstOrDefault<Artwork>("dbo.GetArtworkById",
                                                new { Id = id },
                                                commandType: System.Data.CommandType.StoredProcedure);
        }

        public Artwork Insert(Artwork artwork)
        {
            var parameters = new DynamicParameters();
            parameters.AddDynamicParams(new
            {
                artwork.Title,
                artwork.ArtistId,
                artwork.CategoryId,
                artwork.Year,
                artwork.RentalPrice,
                artwork.Availability,
                artwork.IsAvailable,
                artwork.ArtworkImage
            });

            parameters.Add("Error",
                  dbType: DbType.String,
                  size: 2000,
                  direction: ParameterDirection.Output);

            parameters.Add("RetVal",
                  dbType: DbType.Int32,
                  direction: ParameterDirection.ReturnValue);

            parameters.Add("NewArtworkID",
                  dbType: DbType.Int32,
                  direction: ParameterDirection.Output);

            using var conn = new SqlConnection(_connStr);
            conn.Execute("dbo.udpInsertArtwork",
                        parameters,
                        commandType: CommandType.StoredProcedure);

            var error = parameters.Get<string>("Error");
            var retVal = parameters.Get<int>("RetVal");

            if (retVal > 0)
            {
                throw new Exception($"Insert failed with code {retVal}, error: {error}");
            }

            int newId = parameters.Get<int>("NewArtworkID");
            artwork.ArtworkId = newId;

            return artwork;
        }

        public void Update(Artwork artwork)
        {
            throw new NotImplementedException();
        }

        public int ImportFromXml(string xmlData)
        {
            throw new NotImplementedException();
        }
    }
}
