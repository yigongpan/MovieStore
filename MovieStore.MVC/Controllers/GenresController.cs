using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MoviesStore.Core.ServiceInterfaces;
using MovieStore.Core.ServiceInterfaces;

namespace MovieStore.MVC.Controllers
{
    public class GenresController : Controller
    {
        private readonly IGenreService _genreService;
        //private readonly IMovieService _movieService;
        public GenresController(IGenreService genreService, IMovieService movieService)
        {
            _genreService = genreService;
            //_movieService = movieService;
        }
        public async Task<IActionResult> Index()
        {
            var movies = await _genreService.GetAllGenres();
            return View(movies);
        }

    }
}
