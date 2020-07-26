using Microsoft.Extensions.Caching.Memory;
using MoviesStore.Core.ServiceInterfaces;
using MovieStore.Core.Entities;
using MovieStore.Core.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieStore.Infrastructure.Services
{
    public class GenreService : IGenreService
    {
        //In our application there might be some data that we get from database that wont change often.
        //Genres in our application they don't change much;
        //we display genres in our navigation header, GenreService-->GenreRepository-->Genre table: database call
        //we can use in-memory caching to cache the list of genres for the first time;
        //next time, we just need to check if the cache has Genres list:
            //if yes then take that from cache;if not go to database and put them in cache
        private readonly IGenreRepository _genreRepository;
        //07/23
        private readonly IMemoryCache _memoryCache;
        private static readonly string _genresKey = "genres";
        private static readonly TimeSpan _defaultCacheDuration = TimeSpan.FromDays(30);
        public GenreService(IGenreRepository genreRepository,IMemoryCache memoryCache)
        {
            _genreRepository = genreRepository;
            _memoryCache = memoryCache;
        }

        public async Task<IEnumerable<Genre>> GetAllGenres()
        {
            //return await _genreRepository.GetAllGenres();
            //first check if the cache has genres by key
                                        //if _genresKey has cache,get; if doesn't, create
            var genres = await _memoryCache.GetOrCreateAsync(_genresKey,async entry => {
                entry.SlidingExpiration = _defaultCacheDuration;
                //set data into cache
                return await _genreRepository.GetAllGenres();
            });
            return genres;
        }

        public async Task<IEnumerable<Movie>> GetMoviesByGenre(int id)
        {
            return await _genreRepository.GetMoviesByGenre(id);
        }

        public async Task<IEnumerable<Genre>> GetGenresByMovieId(int id)
        {
            return await _genreRepository.GetGenresByMovie(id);
        }
    }
}
