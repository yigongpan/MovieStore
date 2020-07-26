using Microsoft.AspNetCore.Mvc;
using MoviesStore.Core.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieStore.MVC.Views.Shared.Components.Casts
{
    public class CastsViewComponent:ViewComponent
    {
        private readonly IMovieService _movieService;

        public CastsViewComponent(IMovieService movieService)
        {
            _movieService = movieService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            var casts= await _movieService.GetCasts(id);
            return View(casts);
        }
    }
}
