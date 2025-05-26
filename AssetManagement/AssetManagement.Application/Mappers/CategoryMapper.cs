using AssetManagement.Application.DTOs.Categories;
using AssetManagement.Core.Entities;

namespace AssetManagement.Application.Mappers
{
    public static class CategoryMapper
    {
        public static CategoryResponse MapModelToResponse(this Category category)
        {
            return new CategoryResponse
            {
                Id = category.Id,
                Prefix = category.Prefix,
                CategoryName = category.CategoryName,
                Total = category.Total,
            };
        }
    }
}
