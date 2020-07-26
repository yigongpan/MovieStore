using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace MovieStore.MVC.Helpers
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project

    //Always make sure you register Exceptionmiddlewares at the very beginning (in startup.cs)
    public class MovieStoreExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        //In ASP.NET Core logging is builtin to the framework
        private readonly ILogger<MovieStoreExceptionMiddleware> _logger;

        public MovieStoreExceptionMiddleware(RequestDelegate next,ILogger<MovieStoreExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                _logger.LogInformation("MovieStoreExceptionMiddleware is called");
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Happened:{ex}");
                await HandleException(httpContext, ex);

            }

            //return _next(httpContext);
        }

        public async Task HandleException(HttpContext context, Exception exception)
        {
            //STEP1: as a developer, we need to log the exception details such as:
            //Most popular logging frameworks in .NET: Serilog(preferred),NLog and Log4net(nugget)
            //1.Exception message
            //2.Exception StackTrace
            //3.When the exception happened (Datetime)
            //4.The user info
            //5.Where in our code the exception happened
            _logger.LogInformation($"----------START OF LOGGING------");
            _logger.LogError($"Exception Message:{exception.Message}");
            _logger.LogError($"Exception StackTrace:{exception.StackTrace}");
            _logger.LogInformation($"Exception for User:{context.User.Identity.Name}");
            _logger.LogInformation($"Exception happended on {DateTime.UtcNow}");
            _logger.LogInformation($"----------END OF LOGGING------");

            // 500
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            //STEP2: send notification(Email preferred) to the developer team
            //MailKit(free)  --send emails, SendGrid(paid)

            //STEP3: show a friendly error page to the User
            context.Response.Redirect("/Home/Error");
            await Task.CompletedTask;

        }
    }



    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class MovieStoreExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseMovieStoreExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MovieStoreExceptionMiddleware>();
        }
    }
}
