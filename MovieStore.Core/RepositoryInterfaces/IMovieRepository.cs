using MovieStore.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieStore.Core.RepositoryInterfaces
{
    //07/16
    public interface IMovieRepository:IAsyncRepository<Movie>
    {
        Task<IEnumerable<Movie>> GetHighestRevenueMovies();


        //IAsyncRepository has 8 methods, and IMovieRepository add 1 extra methods;
        //Therefore, any class that implements IMovieRepository interface has to implement all 9 methods;
        //but if this class A at the same time also inherit from EfReposity(a class that already implemented 8 methods), then this class A only need to implement the extra 1 method (that EFRepo didn't implement)



        Task<IEnumerable<Movie>> GetHighestRatedMovies();

        Task<IEnumerable<Cast>> GetCastsByMovieId(int id);

        Task<IEnumerable<Movie>> GetPurchasedMoviesByUser(int id);




    }
}

