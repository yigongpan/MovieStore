using MoviesStore.Core.ServiceInterfaces;
using MovieStore.Core.Entities;
using MovieStore.Core.RepositoryInterfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieStore.Infrastructure.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;
        // Constructor Injection: inject MovieRepository class instance in the place of IMovieRepository interface
        //By using CI, when we want to use a different MovieRepository2, we can tell ASP.NET to inject the MovieRepo2 class here, no need to change any code here
        public MovieService(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }
        public async Task<IEnumerable<Movie>> GetTop25HighestRevenueMovies()
        {
            //test exception MovieStore
            //int a = 0;
            //int b = 2;
            //int c = b / a;
            return await _movieRepository.GetHighestRevenueMovies();
        }

        public async Task<Movie> CreateMovie(Movie movie)
        {
            return await _movieRepository.AddAsync(movie);
        }

        public async Task<Movie> GetMovieById(int id)
        {
            return await _movieRepository.GetByIdAsync(id);
        }

        public async Task<int> GetMoviesCount(string title)
        {
            return await _movieRepository.GetCountAsync(x => x.Title == title);
        }

        //Because we have rating property but not rating column (need to use join Movie and Review Table)
        public async Task<IEnumerable<Movie>> GetTop25HighestRatedMovies()
        {
            return await _movieRepository.GetHighestRatedMovies();
        }

        public async Task<Movie> UpdateMovie(Movie movie)
        {
            return await _movieRepository.UpdateAsync(movie);
        }

        public async Task<IEnumerable<Cast>> GetCasts(int id)
        {
            return await _movieRepository.GetCastsByMovieId(id);
        }


    }

    //Test for the benefit of DI: to see if we can use different data sources
    //public class MovieServiceTest : IMovieService
    //{
    //    public async Task<IEnumerable<Movie>> GetTop25HighestRevenueMovies()
    //    {
    //        var movies = new List<Movie>()
    //                    {
    //                        new Movie {Id = 1, Title = "Avengers: Infinity War", Budget = 1200000},
    //                        new Movie {Id = 2, Title = "Avatar", Budget = 1200000},
    //                        new Movie {Id = 3, Title = "Star Wars: The Force Awakens", Budget = 1200000},
    //                        new Movie {Id = 4, Title = "Titanic", Budget = 1200000},
    //                    };
    //        return movies;
    //    }
    //}
}
