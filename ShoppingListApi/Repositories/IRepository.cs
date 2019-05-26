using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShoppingListApi.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity> FindAsync(int id);
        Task<int> AddAsync(TEntity entity);
        Task<bool> ExistsAsync(int id);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
    }
}
