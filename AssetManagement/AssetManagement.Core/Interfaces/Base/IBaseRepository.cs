using System.Linq;
using System.Threading.Tasks;

namespace AssetManagement.Core.Interfaces.Base
{
    public interface IBaseRepository<T> where T : class
    {
        IQueryable<T> GetAllAsync();
        Task<T?> GetByIdAsync<TId>(TId id);
        Task<T> CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}