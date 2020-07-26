using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
//Add reference
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MoviesStore.Core.ServiceInterfaces;
using MovieStore.Core.RepositoryInterfaces;
using MovieStore.Core.ServiceInterfaces;
using MovieStore.Infrastructure.Data;
using MovieStore.Infrastructure.Repositories;
using MovieStore.Infrastructure.Services;
using MovieStore.MVC.Helpers;
using System;

namespace MovieStore.MVC
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // Day1: This method gets called by the runtime. Use this method to add services to the container.
        //*****07/16 ConfigureServices() is the place to tell ASP.NET to inject class in place of interface
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            //Day2
            services.AddDbContext<MovieStoreDbContext>(options=>
                options.UseSqlServer(Configuration.GetConnectionString("MovieStoreDbConnection")));

            //07/23
            services.AddMemoryCache();

            //07/21
            //tell ASP.NET we are using cookie based authentication
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie
                (
                    //use cookie options, create cookie name, expired time, and routing
                    options =>
                    {
                        options.Cookie.Name = "MovieStoreAuthCookie";
                            //expire time since first login
                        options.ExpireTimeSpan = TimeSpan.FromHours(2);
                                        //if expired, route to Login Action method
                        options.LoginPath = "/Account/Login";
                    }
                );

            //07/16
            //DI in ASP.NET Core has 3 types of Lifetimes: Transient,scoped,Singleton
            services.AddScoped<IMovieRepository, MovieRepository>();
            services.AddScoped<IMovieService, MovieService>();
            //test for DI: this is the only place need to change if we want to change datasource in MovieService(no need to go to controller)
            //tell ASP.NET to inject MovieServiceTest instead of MovieService to IMovieService 
            //services.AddScoped<IMovieService, MovieServiceTest>();

            services.AddScoped<IGenreRepository, GenreRepository>();
            services.AddScoped<IGenreService, GenreService>();
            //07/20
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, UserRepository>();

            //07/22
            services.AddScoped<IPurchaseRepository, PurchaseRepository>();
            services.AddScoped<IFavoriteRepository, FavoriteRepository>();

            services.AddScoped<ICryptoService, CryptoService>();
            services.AddScoped<IReviewRepository, ReviewRepository>();
        }

        // Day 1:This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                //07/22
                //app.UseDeveloperExceptionPage();
                app.UseMovieStoreExceptionMiddleware();

            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();
            //07/22
            //need to add Use Authentication()
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
