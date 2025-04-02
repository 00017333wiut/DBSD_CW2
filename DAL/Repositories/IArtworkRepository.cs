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
            IList<Artwork> Filter(  string? title,
                                     DateTime? availability,
                                     int artistId,
                                     int page = 1,
                                     int pageSize = 10,
                                     string sortColumn = "ArtworkID",
                                     bool sortDesc = false);
            //IEnumerable<Artwork> Filter(string? title,
            //                            int artistId,
            //                            DateTime? availabitity,
            //                            int page,
            //                            int pageSize);
    }
    }

