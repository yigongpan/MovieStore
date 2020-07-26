using Microsoft.AspNetCore.Mvc;
using MoviesStore.Core.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieStore.MVC.Views.Shared.Components.GetGenresByMovie
{
    public class GetGenresByMovieViewComponent : ViewComponent
    {
        private readonly IGenreService _genreService;

        public GetGenresByMovieViewComponent(IGenreService genreService)
        {
            _genreService = genreService;
        }

        public async Task<IViewComponentResult>InvokeAsync(int id)
        {
            var genres = await _genreService.GetGenresByMovieId(id);
            return View(genres);
        }
    }
}
