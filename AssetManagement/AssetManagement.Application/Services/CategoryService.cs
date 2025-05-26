using AssetManagement.Application.DTOs.Categories;
using AssetManagement.Application.Helpers.Params;
using AssetManagement.Application.Interfaces;
using AssetManagement.Application.Mappers;
using AssetManagement.Application.Paginations;
using AssetManagement.Core.Interfaces;

namespace AssetManagement.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public Task<PagedList<CategoryResponse>> GetCategoriesAsync(CategoryParams categoryParams)
        {
            var query = _categoryRepository.GetAllAsync();

            var projectedQuery = query.Select(x => x.MapModelToResponse());

            return PaginationService.ToPagedList(
                projectedQuery,
                categoryParams.PageNumber,
                categoryParams.PageSize
            );
        }
    }
}
