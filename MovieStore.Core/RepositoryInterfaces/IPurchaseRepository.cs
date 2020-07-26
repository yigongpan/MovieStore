using MovieStore.Core.Entities;

namespace MovieStore.Core.RepositoryInterfaces
{
    public interface IPurchaseRepository:IAsyncRepository<Purchase>
    {
        //Task<Purchase> GetPurchaseByIds(int userId, int movieId);
        
    }
}
