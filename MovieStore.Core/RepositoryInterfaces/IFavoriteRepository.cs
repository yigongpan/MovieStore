using MovieStore.Core.Entities;
using System.Threading.Tasks;

namespace MovieStore.Core.RepositoryInterfaces
{
    public interface IFavoriteRepository:IAsyncRepository<Favorite>
    {
        Task DeleteByIds(int userId, int MovieId);
    }
}
