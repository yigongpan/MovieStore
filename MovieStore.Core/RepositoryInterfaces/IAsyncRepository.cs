using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
//add this when we use Async
using System.Threading.Tasks;

namespace MovieStore.Core.RepositoryInterfaces
{
    //interface can be public or internal
    public interface IAsyncRepository<T> where T:class
    {
        //by default Interface method is public, and cannot use access modifer
        //base interface with all our CRUD operations
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> ListAllAsync();
        //07/16 note
        //the parameter needs to be Expression<TDelegate> because it deals with out memory source, so Where() points to IQueryable
        Task<IEnumerable<T>> ListAsync(Expression<Func<T, bool>> filter);
        //optional parameter
        Task<int> GetCountAsync(Expression<Func<T, bool>> filter = null);
        Task<bool> GetExistsAsync(Expression<Func<T, bool>> filter = null);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task DeleteAsync(T entity);

    }
}
