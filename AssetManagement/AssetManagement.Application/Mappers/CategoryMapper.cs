using AssetManagement.Application.DTOs.Category;
using AssetManagement.Core.Entities;
using AssetManagement.Core.Enums;

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
