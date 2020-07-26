using MovieStore.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoviesStore.Core.ServiceInterfaces
{
    //07/17
    public interface IMovieService
    {
        //The numbers of methods depends on client need (out client here is in MVC, the method will be called by controller)
        Task<IEnumerable<Movie>> GetTop25HighestRevenueMovies();
        Task<IEnumerable<Movie>> GetTop25HighestRatedMovies();
        Task<Movie> GetMovieById(int id);
        Task<Movie> CreateMovie(Movie movie);
        Task<Movie> UpdateMovie(Movie movie);
        Task<int> GetMoviesCount(string title = "");
        Task<IEnumerable<Cast>> GetCasts(int id);
    }
}
