using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoviesStore.Core.ServiceInterfaces;
using MovieStore.Core.Models.Request;
using MovieStore.Core.Models.Response;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MovieStore.MVC.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMovieService _movieService;

        public UserController(IUserService userService, IMovieService movieService)
        {
            _userService = userService;
            _movieService = movieService;
        }

        // 1.  Purchase a Movie, HttpPost, store that info in the Purchase table
        // http:localhost:12112/User/Purchase -- HttpPost
        // first check whether the user already bought that movie
        // BUY Buuton in the Movie Details Page will call the above method
        // if user already bought that movie, then replace Buy button with Watch Movie button
        // 2. Get all the Movies Purchased by user, loged in User, take userid from HttpContext and get all the movies
        // and give them to Movie Card partial view
        // http:localhost:12112/User/Purchases -- HttpGet
        // 3. Create a Review for a Movie for Loged In user , take userid from HttpContext and save info in Review Table
        // http:localhost:12112/User/review -- HttpPost
        // Review Button will open a popup and ask user to enter a small review in textarea and have him enter
        // movie rating between 1 and 10 and then save
        // 4. Get all the Reviews done my loged in User,
        // http:localhost:12112/User/reviews -- HttpGet
        // 5. Add a Favorite Movie for Loged In User
        // http:localhost:12112/User/Favorite -- HttpPost
        // add another button called favorite, same conecpt as Purchase
        // FontAweomse libbary and use buttons from there
        // 6.Check if a particular Movie has been added as Favorite by logedin user
        // http:localhost:12112/User/{123}/movie/{12}/favorite  HttpGet
        // 7. Remove favorite
        // http:localhost:12112/User/Favorite -- Httpdelete
        public IActionResult Index()
        {
            return View();
        }



        //07/23
        // Filters in ASP.NET [Attributes]
        // Some piece of code that runs either before an controller or action method executes or when some event happens
        // that run before or after specific stages in the Http Pipeline
        // 1. Authorization ---
        // 2. Action Filter
        // 3. Result Filter
        // 4. Exception filter, but in real world we used Exception middleware to catch exceptions
        // 5. Resource filter
        //  who can call this purchase method???
        // Only Authorized user, user should have entered his un/pw and valid then only we need to execute this method

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Purchase(PurchaseRequestModel purchaseRequestModel)
        {
            //if (User.Identity.IsAuthenticated == false)
            //{
            //    return RedirectToAction("Login", "Account");
            //}
            //else
            //{
                var movie = await _movieService.GetMovieById(purchaseRequestModel.MovieId);
                purchaseRequestModel.UserId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
                //var checkpurchased = await _userService.CheckPurchased(purchaseRequestModel);
                //if hasn't purchased
                //if (checkpurchased == false)
                //{
                    var moviePurchased = await _userService.PurchaseMovie(purchaseRequestModel);
                    return LocalRedirect("~/");
                //}//if has purchased
                //else
                //{
                    //var movieDetails = new MovieDetailsModel
                    //{
                    //    Movie = movie,
                    //    CheckPurchased=checkpurchased
                    //};
                    //movieDetails.Movie = movie;
                    //return View("Views/Movies/Details.cshtml",movieDetails);
                //}


            //}


        }

        [HttpGet]
        public async Task<IActionResult> Purchases()
        {
            var userId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var movies = await _userService.PurchasedMovies(userId);
            return View(movies);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Favorite(FavoriteRequestModel favoriteRequestModel)
        {
            var movie = await _movieService.GetMovieById(favoriteRequestModel.MovieId);
            favoriteRequestModel.UserId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            //var checkfavorited = await _userService.CheckFavorited(favoriteRequestModel);
            var movieDetails = new MovieDetailsModel();
            movieDetails.Movie = movie;
            movieDetails.UserId = favoriteRequestModel.UserId;
            //if hasn't purchased
            //if (checkfavorited == false)
            //{
            await _userService.LikeMovie(favoriteRequestModel.UserId,favoriteRequestModel.MovieId);
            //    movieDetails.CheckFavorited = !checkfavorited;
            //    return View("Views/Movies/Details.cshtml", movieDetails);
            //}//if has purchased
            //else
            //{
            //    movieDetails.CheckFavorited = !checkfavorited;
            //    //之后改!!!!!!!!!!!!!!!!!
            //return LocalRedirect($"/Movies/Details/{movie.Id}");

            //return View("Views/Movies/Details.cshtml", movieDetails);
            return RedirectToAction("Details", "Movies", new { movieid = movieDetails.Movie.Id });
            //}
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DeleteFavorite(FavoriteRequestModel favoriteRequestModel)
        {
            var movie = await _movieService.GetMovieById(favoriteRequestModel.MovieId);
            favoriteRequestModel.UserId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var checkfavorited = await _userService.CheckFavorited(favoriteRequestModel.UserId,favoriteRequestModel.MovieId);
            await _userService.DeleteFavorite(favoriteRequestModel.UserId, favoriteRequestModel.MovieId);
            var movieDetails = new MovieDetailsModel();
            movieDetails.Movie = movie;
            movieDetails.UserId = favoriteRequestModel.UserId;
            //movieDetails.CheckFavorited = !checkfavorited;

            //return View("Views/Movies/Details.cshtml", movieDetails);

            //return LocalRedirect($"/Movies/Details/{movie.Id}");

            return RedirectToAction("Details", "Movies", new { movieid = movieDetails.Movie.Id });

        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Review (UserReviewRequestModel userReviewRequestModel)
        {
            userReviewRequestModel.UserId= Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var newReview = await _userService.Review(userReviewRequestModel);
            return LocalRedirect("~/");
        }

        [Authorize]
        [HttpGet]
        [Route("User/{userId}/movie/{movieId}/favorite")]
        public async Task<bool> CheckFavorite(int userId,int movieId)
        {
            return await _userService.CheckFavorited(userId, movieId);
        }



    }

    
}
