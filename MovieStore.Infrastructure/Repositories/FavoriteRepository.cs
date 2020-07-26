using Microsoft.EntityFrameworkCore;
using MovieStore.Core.Entities;
using MovieStore.Core.RepositoryInterfaces;
using MovieStore.Infrastructure.Data;
using System.Threading.Tasks;

namespace MovieStore.Infrastructure.Repositories
{
    public class FavoriteRepository : EfRepository<Favorite>, IFavoriteRepository
    {
        public FavoriteRepository(MovieStoreDbContext dbContext):base(dbContext)
        {
            
        }

        public async Task DeleteByIds(int userId, int movieId)
        {
            //var movieId = favoriteRequestModel.MovieId;
            //var userId = favoriteRequestModel.UserId;
            var favorite = await _dbContext.Favorites.FirstOrDefaultAsync(f => f.MovieId == movieId && f.UserId == userId);
            await DeleteAsync(favorite);
            

        }
    }
}
