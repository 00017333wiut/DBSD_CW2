using CW2.Models;

namespace CW2.DAL.Entities
{

    public interface IArtworkRepository : IDisposable
    {
        IList<Artwork> GetAll();
        Artwork? GetById(int id);
        Artwork? Insert(Artwork artwork);
        void Update(Artwork artwork);
        void Delete(Artwork artwork);
        string ExportToXml(string? title = null,
                           int? year = null,
                           string sortColumn = "ArtworkID",
                           bool sortDesc = false);
        string ExportToJson(string? title = null,
                           int? year = null,
                           string sortColumn = "ArtworkID",
                           bool sortDesc = false);
        public int ImportFromJson(string jsonData);
        public int ImportFromXml(string xmlData);
        IList<Artwork> Filter(string? title,
                                 DateTime? availability,
                                 int artistId,
                                 int page = 1,
                                 int pageSize = 10,
                                 string sortColumn = "ArtworkID",
                                 bool sortDesc = false);
     
        IList<Artwork> EfFilter(string? title,
                                      string? artistId,
                                      int? categoryId,
                                      int? year,
                                      decimal? minRentalPrice,
                                      decimal? maxRentalPrice,
                                      bool? isAvailable,
                                      int page = 1,
                                      int pageSize = 10,
                                      string sortColumn = "ArtworkID",
                                      bool sortDesc = false);
        void IDisposable.Dispose()
        {
            //no implementation
        }



    }
}

