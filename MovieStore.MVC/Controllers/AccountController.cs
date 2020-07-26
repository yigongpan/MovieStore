using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MoviesStore.Core.ServiceInterfaces;
using MovieStore.Core.Models.Request;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MovieStore.MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        //need 1 empty page with GET to show User and another one with POST to get the data from user
        [HttpGet]
        public ActionResult Register()
            //when we add View, use the template of ASP.NET (Create, and then using the model we created)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterRequestModel userRegisterRequestModel)
        {
            //when every properties (in UserRegisterRequestModel) rule pass, will be true
            if (ModelState.IsValid)
            {
                // now call the service
                var createdUser = await _userService.RegisterUser(userRegisterRequestModel);
                //go to the Login Action Method
                return RedirectToAction("Login");
            }
            // we take this object from the View
            return View();
        }


        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestModel loginRequest)
        {
            //07/21
            //Http is stateless, each and every request is independent of each other
            //e.g
            //10 AM user1--> http:localhost / movies / index
            //10 AM user2--> http:localhost / movies / index
            //10 AM user3--> http:localhost / movies / index
            //10:01AM user1 go to / account / login, we can create an authenticate cookie
            //cookie is one way of string information on browser; others are local storage and session storage
            //if there are any cookies present then those cookies will be automatically sent to server.
            //think of this cookie as movie ticket, present it to cinema(server),and you can get in and watch.
            //10:02AM user1 go to / user / purchases->we are expecting a page that shows movies bought by user1;
            //but before that we need to check if the cookie is expired or valid or not.
            //Cookies is one way of state management, client side
            //07/20
            if (ModelState.IsValid)
            {
                // call service layer to validate user
                var user = await _userService.ValidateUser(loginRequest.Email, loginRequest.Password);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid Login");
                }


                //07/21
                //we want to show FirstName, LastName on header(Navigation) of cookie.
                //Claim can help us collect these information, and we can add Claims to the cookie
                //donot put sensitive information on cookie
                //create Claims based on you application needs
                var claims = new List<Claim>
                {
                    //there are many options of ClaimTypes
                    
                    new Claim(ClaimTypes.GivenName, user.FirstName),
                    new Claim(ClaimTypes.Surname, user.LastName),
                    //ClaimTypes are all string, so we need to convert int to String here            
                    new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                    new Claim(ClaimTypes.Name,  user.Email),
                };



                // we need to create an Identity Object to hold those claims
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                //finally we need to create a cookie that will be attached to the Http Response
                // ♥ HttpContext is probably most important class in ASP.NET, that holds all the information regarding that Http Request/Response
                //when creating a cookie, the following line will automatically encrypt all the claimsIdentity information
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                // Once ASP.NET Craetes Authntication Cookies, it will check for that cookie in the HttpRequest and see if the cookie is not expired
                // and it will decrypt the information present in the cookie to check whether User is Authenticated and will also get claims from the cookies
                //(the server will ask browser for cookie authentication)

                //to mannually add cookie
                //HttpContext.Response.Cookies.Append("userLanguage", "English");

                //redirect to home page (from localhost:12312/account/ to localhost:12312  , 1 level up, so we use ~)
                return LocalRedirect("~/");
            }
            return View();



        }

        //Logout by deleting the cookie
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return LocalRedirect("~/");
        }
    }
}
