using MovieStore.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieStore.Core.RepositoryInterfaces
{
    public interface IGenreRepository:IAsyncRepository<Genre>
    {
        Task<IEnumerable<Genre>> GetAllGenres();
        Task<IEnumerable<Movie>> GetMoviesByGenre(int id);
        Task<IEnumerable<Genre>> GetGenresByMovie(int id);
    }
}
