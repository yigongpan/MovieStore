using MovieStore.Core.Entities;
using MovieStore.Core.RepositoryInterfaces;
using MovieStore.Infrastructure.Data;

namespace MovieStore.Infrastructure.Repositories
{
    public class PurchaseRepository : EfRepository<Purchase>, IPurchaseRepository
    {

        public PurchaseRepository(MovieStoreDbContext dbContext):base(dbContext)
        {

        }


        //{
        //    return await _dbContext.Purchases.Where(p => p.UserId == userId && p.MovieId == movieId).FirstOrDefaultAsync();
        //}
    }
}
