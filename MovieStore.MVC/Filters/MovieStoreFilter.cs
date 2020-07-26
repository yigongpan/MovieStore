using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieStore.MVC.Filters
{

    //07/23
    public class MovieStoreFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            //execute after an action method executes
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //will execute this method before an action method executes

            //e.g we wanna track the information how man people been to Movie Details page
            var data = context.HttpContext.Request.Path;
            //see if the user is authenticated user or not.
            var someotherdata = context.HttpContext.User.Identity.IsAuthenticated;
            //we can log these information in text files or database
        }
    }
}
