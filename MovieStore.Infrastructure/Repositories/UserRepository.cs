using Microsoft.EntityFrameworkCore;
using MovieStore.Core.Entities;
using MovieStore.Core.RepositoryInterfaces;
using MovieStore.Infrastructure.Data;
using System.Threading.Tasks;

namespace MovieStore.Infrastructure.Repositories
{
    public class UserRepository : EfRepository<User>, IUserRepository
    {
        public UserRepository(MovieStoreDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<User> GetUserByEmail(string email)
        {
            //FirstOrDefault(): if we can find the record, return the first record
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
