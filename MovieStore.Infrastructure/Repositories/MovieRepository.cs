using Microsoft.EntityFrameworkCore;
using MovieStore.Core.Entities;
using MovieStore.Core.RepositoryInterfaces;
using MovieStore.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace MovieStore.Infrastructure.Repositories
{
    //put inherit class first, then interface
    public class MovieRepository : EfRepository<Movie>, IMovieRepository
    {
        public MovieRepository(MovieStoreDbContext dbContext) : base(dbContext)
        {
        }


        public async Task<IEnumerable<Movie>> GetHighestRevenueMovies()
        {
            //has to add ToListAsync() because the previous expression returns IQueryable<Movie>,which is not awaitable (we can only await Task or some awaiter)
            var movies = await _dbContext.Movies.OrderByDescending(m => m.Revenue).Take(25).ToListAsync();
            // select top 25 from Movies order by Revenue desc;
            return movies;
        }

        public async Task<IEnumerable<Movie>> GetHighestRatedMovies()
        {
            var movies = await _dbContext.Movies.OrderByDescending(m => m.Reviews.Average(r => r.Rating)).Take(25).ToListAsync();
            return movies;
        }



        //override the method, and gather more information inside this movie (rating,characters)
        public override async Task<Movie> GetByIdAsync(int id)
        {
            var movie = await _dbContext.Movies
                                        .Include(m => m.MovieCasts).ThenInclude(m => m.Cast)
                                        .Include(m => m.MovieGenres).ThenInclude(m => m.Genre)
                                        .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null) return null;
            var movieRating = await _dbContext.Reviews.Where(r => r.MovieId == id).AverageAsync(r => r.Rating);
            if (movieRating > 0) movie.Rating = Math.Round(movieRating, 1);
            return movie;
        }

        public async Task<IEnumerable<Cast>> GetCastsByMovieId(int id)
        {
            var casts = await _dbContext.MovieCasts
                                .Where(mc => mc.MovieId == id)
                                .Include(mc => mc.Movie)
                                .Include(mc => mc.Cast)
                                .Select(mc => mc.Cast)
                                .ToListAsync();
            return casts;


            //尝试1
            //public async Task<IEnumerable<Movie>> CalculateRating()
            //{
            //    var movies = await _dbContext.Reviews.Include(r => r.Movie).GroupBy(r => r.MovieId).OrderByDescending(g => g.Average(m => m.Rating))
            //                .Select(r => new Movie { Id = r.Key, Rating = r.Average(x => x.Rating) }).ToListAsync();
            //    return movies;

            //    //var movies = from m in _dbContext.Movies
            //    //         join r in _dbContext.Reviews
            //    //         on m.Id equals r.MovieId
            //    //         group r by r.MovieId into mr
            //    //         select new Movie{ Id = mr.Key., Rating = mr.Average(_dbContext.Reviews=>_dbContext.Reviews.) };
            //}
            //await _dbContext.Movies.Attach(;
            //await _dbContext.Movies.Entry(user).Property(x => x.Password).IsModified = true;
            //db.SaveChanges();
            //}

        }

        public async Task<IEnumerable<Movie>> GetPurchasedMoviesByUser(int id)
        {
            var pmovies = await _dbContext.Purchases.Include(p => p.Movie).Where(p => p.UserId == id).ToListAsync();
            var movies = new Collection<Movie>();
            foreach (var item in pmovies)
            {
                movies.Add(item.Movie);
            }

            return movies;
        }


    }
}
