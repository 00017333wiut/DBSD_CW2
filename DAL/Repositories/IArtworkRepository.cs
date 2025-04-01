using CW2.Models;

namespace CW2.DAL.Entities
{
   
        public interface IArtworkRepository
        {
            IList<Artwork> GetAll();
            Artwork? GetById(int id);
            Artwork? Insert(Artwork artwork);
            void Update(Artwork artwork);
            void Delete(Artwork artwork);
            IList<Artwork> Filter(string? title,
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
        }
    }

