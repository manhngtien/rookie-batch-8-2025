using AssetManagement.Application.DTOs.Categories;
using AssetManagement.Application.Helpers.Params;
using AssetManagement.Application.Paginations;

namespace AssetManagement.Application.Interfaces
{
    public interface ICategoryService
    {
        public Task<PagedList<CategoryResponse>> GetCategoriesAsync(CategoryParams categoryParams);
    }
}
