using Microsoft.AspNetCore.Mvc;
using MoviesStore.Core.ServiceInterfaces;
using MovieStore.MVC.Filters;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MovieStore.Core.Models.Response;

namespace MovieStore.MVC.Controllers
{
    public class MoviesController : Controller
    {


        //07/11?
        //public class MoviesController : Controller
        //{
        //GET localhost/Movies/index

        //public IActionResult Index()
        //{
        //    //go to database and get some list of movies and give it to the view

        //    var movies = new List<Movie>
        //    {
        //        new Movie {Id = 1, Title = "Avengers: Infinity War", Budget = 1200000},
        //        new Movie {Id = 2, Title = "Avatar", Budget = 1200000},
        //        new Movie {Id = 3, Title = "Star Wars: The Force Awakens", Budget = 1200000},
        //        new Movie {Id = 4, Title = "Titanic", Budget = 1200000},
        //        new Movie {Id = 5, Title = "Inception", Budget = 1200000},
        //        new Movie {Id = 6, Title = "Avengers: Age of Ultron", Budget = 1200000},
        //        new Movie {Id = 7, Title = "Interstellar", Budget = 1200000},
        //        new Movie {Id = 8, Title = "Fight Club", Budget = 1200000},

        //    };

        //        //ViewBag is a dynamic type,checks on run-time
        //        //we can use ViewBag.xxx to send data to view
        //        //or ViewData
        //        ViewBag.MoviesCount = movies.Count;
        //        ViewData["myname"] = "John Doe";
        //        //compile time checks vs run-time checks


        //        //why people prefer strongly typed language
        //        //we want to detect errors asap?
        //        //e.g
        //        //var list = new List<int>();
        //        //list.Add(asdasd);
        //        //when we use add() we can see hint about what type should be inside the ()--in this case is int type

        //        //3 ways to send data from Controller to View
        //        //1.Strongly-Typed models (preferred)
        //        // 2. ViewBag --dynamic
        //        // 3. ViewData - key/value


        //        //we need to pass data from Contorller action method to the view
        //        //usually its preferred to send a strongly typed Model or object to the view
        //    return View(movies);
        //}

        //    [HttpPost] //by default, this attribute can only be on top of method,not class (but Route can do)
        //    public IActionResult Create(string title, decimal budget)
        //    {
        //        //POST //http://localhost/Movies/create

        //        //Model binding is case in-sensitive
        //        //   is case in-sensitive, look at incoming request and maps the input elements name/value with the parameter names of the action method,
        //        //then the parameter will have the value automatically; it will also does casting / converting

        //        //in [HttpPost] method, as long as the parameter in this Create() method is the same as the name attribute (in this case is inputbox name) inside Create.cshtml , 
        //        //the value input by user can be passed this Create() method parameter

        //        //we need to use the data from the view and save it in database

        //        return View();
        //    }

        //    [HttpGet]
        //    public IActionResult Create()

        //    {
        //        //GET //http://localhost/Movies/create
        //        // we need to have this method so that we can show the empty page for user to enter Movie information that needs to be created
        //        ViewData["myname"] = "John Doe";
        //        return View();
        //    }
        //}

        //07/16-------------------
        //[HttpGet]
        //public async Task<IActionResult> Index()
        //{
        // Thread1
        //  var movieService = new MovieService();
        // 5 seconds var movies = await movieService.GetAllMovie();  I/O bound operation
        // when using await keyword, the method will return a Task type for you;
        //instead of waiting for 5 secs here,thread1 will return to threadpool and be available to other task
        //when we use async keyword on a method without using await, then this method will function as a normal synchronous method.

        //Benefits:
        // Improving the scalability of the application, so that your application can serve many concurrent requests properly
        // async/await will prevent thread starvation scenario (in sync method, 100 threads available while 101 requests example).
        // async make it I/O bound operation rather than CPU
        // MS recommend using async for I/O bound operations: Database calls, Http calls, over network
        // Every Async method will return Task type (like Task<Movie>, Task<int>) or return nothing (like Task)
        // in your C# or any Library whenever you see a method with Async in the method name, that means you can await that method
        // EF has two kind of methods, normal sync method, async methods...
        //    return View();
        //}

        //07/16 part 2
        //IOC (Inversion of Control)
        //,ASP.NET Core has built-in IOC/DI
        //but In .NET Framework we need to reply on 3rd party IOC (Autofac,Ninject) to do DI.
        //In ASP.NET Core, we need to go to startUp.cs tell the ASP.NET to inject class instance to the place of interface
        private readonly IMovieService _movieService;
        private readonly IGenreService _genreService;
        private readonly IUserService _userService;
        public MoviesController(IMovieService movieService, IGenreService genreService,IUserService userService)
        {
            _movieService = movieService;
            _genreService = genreService;
            _userService = userService;
        }

        //  GET localhost/Movies/index
        [HttpGet]
        public async Task<IActionResult> GetHighestRevenueMovies()
        {
            // call our Movie Service ,method, highest grossing method
            var movies = await _movieService.GetTop25HighestRevenueMovies();
            return View(movies);
        }

        [HttpGet]
        public async Task<IActionResult> GetHighestRatingMovies()
        {
            var movies = await _movieService.GetTop25HighestRatedMovies();
            return View(movies);
        }

        //设置点击一个genre后，显示该genre的所有电影
        //方法1:
        //[HttpGet]
        //[Route("Movies/Genre/{genreId}")]
        //public async Task<IActionResult> Genre(int genreId)
        //{
        //    var movies = await _genreService.GetMoviesByGenre(genreId);
        //    return View(movies);
        //}

        //方法2:
        //parameter genreId must be the same as Default.cshtml里 asp-route-{value} 的value (asp-route-genreId)
        [HttpGet]
        public async Task<IActionResult> Genre(int genreId)
        {
            var movies = await _genreService.GetMoviesByGenre(genreId);
            return View(movies);
        }

        [MovieStoreFilter]
        [HttpGet]
        //[Route("Movies/Details/{movieId}")]
        public async Task<IActionResult> Details(int movieId)
        {
            //var purchaseRequestModel = new PurchaseRequestModel();
            //var favoriteRequestModel = new FavoriteRequestModel();
            var movieDetails = new MovieDetailsModel();

            if (User.Identity.IsAuthenticated) {
                var userId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
                movieDetails.UserId = userId;
                movieDetails.MovieId = movieId;
                movieDetails.CheckPurchased = await _userService.CheckPurchased(userId,movieId);
                movieDetails.CheckFavorited = await _userService.CheckFavorited(userId, movieId);
                movieDetails.CheckReviewed = await _userService.CheckReviewed(userId, movieId);


            }
            var movie = await _movieService.GetMovieById(movieId);
            movieDetails.Movie = movie;
            return View(movieDetails);
        }







    } 
}
