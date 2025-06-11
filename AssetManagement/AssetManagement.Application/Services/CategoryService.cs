using AssetManagement.Application.DTOs.Categories;
using AssetManagement.Application.Exceptions;
using AssetManagement.Application.Interfaces;
using AssetManagement.Application.Mappers;
using AssetManagement.Core.Entities;
using AssetManagement.Core.Exceptions;
using AssetManagement.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AssetManagement.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<CategoryResponse>> GetCategoriesAsync()
        {
            var query = _categoryRepository.GetAllAsync();

            var projectedQuery = await query
                .Select(x => x.MapModelToResponse())
                .ToListAsync();

            return projectedQuery;
        }

        public async Task<CategoryResponse?> GetCategoryByIdAsync(int categoryId)
        {
            var category = await _categoryRepository.GetByIdAsync(categoryId);
            if (category is null)
            {
                var attributes = new Dictionary<string, object>
                {
                    { "categoryId", categoryId }
                };
                throw new AppException(ErrorCode.CATEGORY_NOT_FOUND, attributes);
            }

            return category.MapModelToResponse();
        }

        public async Task<CategoryResponse> CreateCategoryAsync(CreateCategoryRequest createCategoryRequest)
        {
            if (await _categoryRepository.FindCategoryByName(createCategoryRequest.CategoryName))
            {
                var attributes = new Dictionary<string, object>
                {
                    {"categoryName", createCategoryRequest.CategoryName}
                };

                throw new AppException(ErrorCode.CATEGORY_NAME_ALREADY_EXISTS, attributes);
            }
            
            if (await _categoryRepository.FindCategoryByPrefix(createCategoryRequest.Prefix))
            {
                var attributes = new Dictionary<string, object>
                {
                    {"prefix", createCategoryRequest.Prefix}
                };

                throw new AppException(ErrorCode.CATEGORY_PREFIX_ALREADY_EXISTS, attributes);
            }

            var category = new Category
            {
                CategoryName = createCategoryRequest.CategoryName,
                Prefix = createCategoryRequest.Prefix,
                Total = 0
            };

            var createdCategory = await _categoryRepository.CreateAsync(category);
            await _unitOfWork.CommitAsync();
            
            return createdCategory.MapModelToResponse();
        }
    }
}
