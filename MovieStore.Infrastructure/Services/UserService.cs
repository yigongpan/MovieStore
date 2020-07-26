using MoviesStore.Core.ServiceInterfaces;
using MovieStore.Core.Entities;
using MovieStore.Core.Models.Request;
using MovieStore.Core.Models.Response;
using MovieStore.Core.RepositoryInterfaces;
using MovieStore.Core.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieStore.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMovieRepository _movieRepository;
        private readonly IFavoriteRepository _favoriteRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly ICryptoService _cryptoService;
        private readonly IMovieService _movieService;
        public UserService(IUserRepository userRepository,IPurchaseRepository purchaseRepository,ICryptoService cryptoService,IMovieService movieService,
            IMovieRepository movieRepository,IFavoriteRepository favoriteRepository,IReviewRepository reviewRepository)
        {
            _userRepository = userRepository;
            _purchaseRepository = purchaseRepository;
            _movieRepository = movieRepository;
            _cryptoService = cryptoService;
            _movieService = movieService;
            _favoriteRepository = favoriteRepository;
            _reviewRepository = reviewRepository;
        }

        public async Task<UserRegisterResponseModel> RegisterUser(UserRegisterRequestModel requestModel)
        {
            // Step 1 : Check whether this user already exists in the database
            var dbUser = await _userRepository.GetUserByEmail(requestModel.Email);
            if (dbUser != null)
            {
                // we already have this user(email) in our table
                // return or throw an exception saying Conflict user
                throw new Exception("User already registered, Please try to Login");
            }
            // Step 2 : lets Generate a random unique Salt
            var salt = _cryptoService.GenerateSalt();
            // Never ever craete your own Hashing Algorithm, always use Industry tested/tried Hashing Algorithm
            // Step 3: we  hash the password with the salt created in the above step
            var hashedPassword = _cryptoService.HashPassword(requestModel.Password, salt);
            // craete User object so that we can save it to User Table
            var user = new User
            {
                Email = requestModel.Email,
                Salt = salt,
                HashedPassword = hashedPassword,
                FirstName = requestModel.FirstName,
                LastName = requestModel.LastName
            };
            // Step 4: Save it to Database
            var createdUser = await _userRepository.AddAsync(user);
            var response = new UserRegisterResponseModel
            {
                Id = createdUser.Id,
                Email = createdUser.Email,
                FirstName = createdUser.FirstName,
                LastName = createdUser.LastName
            };
            return response;
        }

        public async Task<UserLoginResponseModel> ValidateUser(string email, string password)
        {
            // Step 1 : Get user record from the databse by email;
            var user = await _userRepository.GetUserByEmail(email);
            if (user == null)
            {
                // user does not even exists
                throw new Exception("Register first, user does not exists");
            }
            // Step 2: we need to hash the password that user entered in the page with Salt from the database from step1
            var hashedPassword = _cryptoService.HashPassword(password, user.Salt);
            // Step 3 : Compare the databse hashed password with Hashed passowrd genereated in step 2
            if (hashedPassword == user.HashedPassword)
            {
                // user entered right password
                // send some user details
                var response = new UserLoginResponseModel
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    DateOfBirth = user.DateOfBirth,
                    Email = user.Email
                };
                return response;
            }
            return null;
        }

        
        public async Task<Purchase> PurchaseMovie(PurchaseRequestModel purchaseRequestModel)
        {

            var movie = await _movieService.GetMovieById(purchaseRequestModel.MovieId);
            var p = new Purchase
            {
                UserId = purchaseRequestModel.UserId,
                PurchaseNumber = purchaseRequestModel.PurchaseNumber.Value,
                TotalPrice = movie.Price.Value,
                MovieId = purchaseRequestModel.MovieId,
                PurchaseDateTime = purchaseRequestModel.PurchaseDate.Value
            };

            return await _purchaseRepository.AddAsync(p);
        }

        public async Task<bool> CheckPurchased(int userId,int movieId)
        {
            //var check = await _purchaseRepository.GetExistsAsync(p => p.UserId == purchaseRequestModel.UserId && p.MovieId == purchaseRequestModel.MovieId);
            var check = await _purchaseRepository.GetExistsAsync(p => p.UserId == userId && p.MovieId == movieId);
            return check;
        }

        public async Task<IEnumerable<Movie>> PurchasedMovies(int id)
        {
            return await _movieRepository.GetPurchasedMoviesByUser(id);
        }

        public async Task<Favorite> LikeMovie(int userId, int movieId)
        {
            //var movie = await _movieService.GetMovieById(favoriteRequestModel.MovieId);
            var f = new Favorite
            {

                UserId = userId,
                MovieId=movieId
            };

            return await _favoriteRepository.AddAsync(f);
        }

        public async Task<bool> CheckFavorited(int userId, int movieId)
        {
            //var check = await _favoriteRepository.GetExistsAsync(p => p.UserId == favoriteRequestModel.UserId && p.MovieId == favoriteRequestModel.MovieId);
            var check = await _favoriteRepository.GetExistsAsync(p => p.UserId == userId && p.MovieId == movieId);
            return check;
        }

        public async Task DeleteFavorite(int userId, int movieId)
        {
            //await _favoriteRepository.DeleteByIds(favoriteRequestModel);
            await _favoriteRepository.DeleteByIds(userId, movieId);
        }

        public async Task<Review> Review(UserReviewRequestModel userReviewRequestModel)
        {
            var review = new Review
            {
                MovieId = userReviewRequestModel.MovieId,
                UserId = userReviewRequestModel.UserId,
                Rating = userReviewRequestModel.Rating,
                ReviewText = userReviewRequestModel.ReviewText
            };
            return await _reviewRepository.AddAsync(review);
        }

        public async Task<bool> CheckReviewed(int userId, int movieId)
        {
            var check = false;
            //var check = await _favoriteRepository.GetExistsAsync(p => p.UserId == favoriteRequestModel.UserId && p.MovieId == favoriteRequestModel.MovieId);
            if (await CheckPurchased(userId,movieId))
            {
                check = await _reviewRepository.GetExistsAsync(p => p.UserId == userId && p.MovieId == movieId);
            }

            return check;
        }
    }
}