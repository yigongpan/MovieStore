using MovieStore.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoviesStore.Core.ServiceInterfaces
{
    public interface IGenreService
    {
        Task<IEnumerable<Genre>> GetAllGenres();
        Task<IEnumerable<Movie>> GetMoviesByGenre(int id);
        Task<IEnumerable<Genre>> GetGenresByMovieId(int id);
    }
}
