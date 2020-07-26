using Microsoft.EntityFrameworkCore;
using MovieStore.Core.RepositoryInterfaces;
using MovieStore.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MovieStore.Infrastructure.Repositories
{
    //07/16
    //we are going to use EF in this repository
    public class EfRepository<T>:IAsyncRepository<T> where T:class
    {
        protected readonly MovieStoreDbContext _dbContext;
        public EfRepository(MovieStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> ListAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> ListAsync(Expression<Func<T, bool>> filter)
        {
            //check the explanation on .Where()
            return await _dbContext.Set<T>().Where(filter).ToListAsync();
        }

        public virtual async Task<int> GetCountAsync(Expression<Func<T, bool>> filter = null)
        {
            //当没有筛选条件时
            if (filter != null)
            {
                return await _dbContext.Set<T>().Where(filter).CountAsync();
            }
            return await _dbContext.Set<T>().CountAsync();
        }

        public virtual async Task<bool> GetExistsAsync(Expression<Func<T, bool>> filter = null)
        {
            //&&短路与  前面为false的话后面语句不会执行;前面为true的话后面语句会执行
                return filter!=null && await _dbContext.Set<T>().Where(filter).AnyAsync();

        }

        public async Task<T> AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            //**for CRUD operation we need to use SaveChangesAsync() method to save in database
            await _dbContext.SaveChangesAsync();
            //show newly created entity to U/I; could also be bool or int to show the transaction is done
            return entity;
        }

        public async Task<T> UpdateAsync(T entity)
        {
            //remember this method
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public virtual async Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }












    }
}
