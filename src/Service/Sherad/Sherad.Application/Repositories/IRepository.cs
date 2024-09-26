using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sherad.Application.Repositories
{
    public interface IRepository<T, TKey>
        where T : notnull 
        where TKey : notnull
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetAsync(TKey id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(TKey id);

        Task SaveAsync(CancellationToken token = default);
    }
}
