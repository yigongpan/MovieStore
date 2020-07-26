using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MovieStore.Core.ServiceInterfaces
{
    public interface IFavoriteService
    {
        Task<bool> UserMovieFavorite(int userId, int movieId);
    }
}
