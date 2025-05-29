using AssetManagement.Application.DTOs.Categories;

namespace AssetManagement.Application.Interfaces
{
    public interface ICategoryService
    {
        public Task<List<CategoryResponse>> GetCategoriesAsync();
        public Task<CategoryResponse?> GetCategoryByIdAsync(int categoryId);
        public Task<CategoryResponse> CreateCategoryAsync(CreateCategoryRequest createCategoryRequest);
    }
}
