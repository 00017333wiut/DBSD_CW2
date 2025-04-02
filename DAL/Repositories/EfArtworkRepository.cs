using CW2.DAL.EF;
using CW2.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CW2.DAL.Repositories
{
    public class EfArtworkRepository : IArtworkRepository
    {
        private readonly ArtworkDbContext _dbContext;

        public EfArtworkRepository(ArtworkDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Delete(Artwork artwork)
        {
            _dbContext.Remove(artwork);
            _dbContext.SaveChanges();
        }

        public IList<Artwork> Filter(string? title,
                                     string? artistId,
                                     int? categoryId,
                                     int? year,
                                     decimal? minRentalPrice,
                                     decimal? maxRentalPrice,
                                     bool? isAvailable,
                                     int page = 1,
                                     int pageSize = 10,
                                     string sortColumn = "ArtworkID",
                                     bool sortDesc = false)
        {
            var query = _dbContext.Artworks.AsQueryable();

            if (!string.IsNullOrWhiteSpace(title))
            {
                query = query.Where(a => a.Title.StartsWith(title));
            }

            if (!string.IsNullOrWhiteSpace(artistId))
            {
                query = query.Where(a => a.ArtistId.ToString() == artistId);
            }

            if (categoryId.HasValue)
            {
                query = query.Where(a => a.CategoryId == categoryId.Value);
            }

            if (year.HasValue)
            {
                query = query.Where(a => a.Year >= year.Value);
            }

            if (minRentalPrice.HasValue)
            {
                query = query.Where(a => a.RentalPrice >= minRentalPrice.Value);
            }

            if (maxRentalPrice.HasValue)
            {
                query = query.Where(a => a.RentalPrice <= maxRentalPrice.Value);
            }

            if (isAvailable.HasValue)
            {
                query = query.Where(a => a.IsAvailable == isAvailable.Value);
            }

            // Sorting using nameof()
            if (nameof(Artwork.Title).Equals(sortColumn))
            {
                query = sortDesc
                    ? query.OrderByDescending(a => a.Title)
                    : query.OrderBy(a => a.Title);
            }
            else if (nameof(Artwork.ArtistId).Equals(sortColumn))
            {
                query = sortDesc
                    ? query.OrderByDescending(a => a.ArtistId)
                    : query.OrderBy(a => a.ArtistId);
            }
            else if (nameof(Artwork.CategoryId).Equals(sortColumn))
            {
                query = sortDesc
                    ? query.OrderByDescending(a => a.CategoryId)
                    : query.OrderBy(a => a.CategoryId);
            }
            else if (nameof(Artwork.Year).Equals(sortColumn))
            {
                query = sortDesc
                    ? query.OrderByDescending(a => a.Year)
                    : query.OrderBy(a => a.Year);
            }
            else if (nameof(Artwork.RentalPrice).Equals(sortColumn))
            {
                query = sortDesc
                    ? query.OrderByDescending(a => a.RentalPrice)
                    : query.OrderBy(a => a.RentalPrice);
            }
            else if (nameof(Artwork.IsAvailable).Equals(sortColumn))
            {
                query = sortDesc
                    ? query.OrderByDescending(a => a.IsAvailable)
                    : query.OrderBy(a => a.IsAvailable);
            }
            else
            {
                // Default sorting by ArtworkID
                query = sortDesc
                    ? query.OrderByDescending(a => a.ArtworkId)
                    : query.OrderBy(a => a.ArtworkId);
            }


            // Pagination
            return query.Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();
        }

        public IList<Artwork> GetAll()
        {
            return _dbContext.Artworks.ToList();
        }

        public Artwork? GetById(int id)
        {
            return _dbContext.Artworks.Find(id);
        }

        public Artwork? Insert(Artwork artwork)
        {
            var insertedart = _dbContext.Add(artwork).Entity;
            _dbContext.SaveChanges();

            return insertedart;
        }

        public void Update(Artwork artwork)
        {
            _dbContext.Update(artwork);
            _dbContext.SaveChanges();
        }

        public void Dispose()
        {
            _dbContext?.Dispose();
        }

    }
}
