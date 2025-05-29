using AssetManagement.Core.Entities;
using AssetManagement.Core.Interfaces.Base;

namespace AssetManagement.Core.Interfaces
{
    public interface ICategoryRepository : IBaseRepository<Category>
    {
        Task<Category?> GetByIdAsync(int categoryId);
        Task<bool> FindCategoryByName(string categoryName);
        Task<bool> FindCategoryByPrefix(string prefix);
    }
}
