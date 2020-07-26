using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MovieStore.Core.Entities;
using MoviesStore.Core.ServiceInterfaces;
using MovieStore.MVC.Models;

namespace MovieStore.MVC.Controllers
{
    //Day 1
    //Any C# class can become a MVC Controller if it inherits from Controller base class from Microsoft.AspNetCore.Mvc
    
    //http://localhost:2323/home/index

    //HomeController
    //Index--Action method
    public class HomeController : Controller
    {
        //07/17
        private readonly IMovieService _movieService;
        public HomeController(IMovieService movieService)
        {
            _movieService = movieService;
        }
        ////create action method
        public async Task<IActionResult> Index()
        {
            //Day 1 {
            //return a instance of a class that implements that IActionResult
            //View() is a method that returns ViewResult (which implements IActionResult)
            //By default MVC will look for same View name as Action method name
            //it will look inside Views folder -->Home(same name as Controller)-->Index.cshtml

            //1.Program.cs-->Main method
            //2.StartUp Class (app.UseStaticFiles,app.... they are middlewares  app.UseEndpoints use routing)
            //3.ConfigureServices
            //4.Configure
            //5.HomeController
            //6.Action method
            //7.return a View

            //ASP.NET Core Middleware-- a piece of software logic that will be executed
            //the order of middlewares matters
            //e.g Train--DC to Boston
            //DC 20people,multiple stops,boston
            //request -->Middleware1--some process-->next M2-->...M5-->Response
            //}


            //07/17
            //we need a Movie card, which will be used in multiple places
            //1.Home Page--show top revenue movies-->Movie Card
            //2.Genres:    show Movies belonging to that genre --> Movie Card
            //3.Top Rated Movies --Top Movies as a card
            //we need to create a Movie Card that can be reused in different places (URL)
            //-->Partial View
            //Partial View will help us in creating reusable Views across the application
            //Partial Views are views inside another view
            var movies = await _movieService.GetTop25HighestRevenueMovies();
            return View(movies);


        }


        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }



    }
}
