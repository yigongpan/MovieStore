using Microsoft.EntityFrameworkCore;
using MovieStore.Core.Entities;
using MovieStore.Core.RepositoryInterfaces;
using MovieStore.Infrastructure.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieStore.Infrastructure.Repositories
{
    public class GenreRepository : EfRepository<Genre>, IGenreRepository
    {
        public GenreRepository(MovieStoreDbContext dbContext):base(dbContext)
        {

        }

        public async Task<IEnumerable<Genre>> GetAllGenres()
        {
            var genres = await _dbContext.Genres.OrderBy(g => g.Id).ToListAsync();
            return genres;
        }



        public async Task<IEnumerable<Movie>> GetMoviesByGenre(int genreId)
        {
            //use Include to include the navigation property table's data (like join table)
            var movies = await _dbContext.MovieGenres.Where(g => g.GenreId == genreId).Include(mg => mg.Movie)
                                         .Select(m => m.Movie)
                                         .ToListAsync();
            return movies;

        }

        public async Task<IEnumerable<Genre>> GetGenresByMovie(int id)
        {
           return await _dbContext.MovieGenres.Include(mg => mg.Genre).Where(mg => mg.MovieId == id).Select(mg=>mg.Genre).ToListAsync();
        }
    }
}
