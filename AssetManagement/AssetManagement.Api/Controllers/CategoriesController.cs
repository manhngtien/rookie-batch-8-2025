using AssetManagement.Api.Controllers.Base;
using AssetManagement.Api.Extensions;
using AssetManagement.Application.DTOs.Categories;
using AssetManagement.Application.Helpers.Params;
using AssetManagement.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssetManagement.Api.Controllers
{
    public class CategoriesController : BaseApiController
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService assetService)
        {
            _categoryService = assetService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<CategoryResponse>>> GetCategories([FromQuery] CategoryParams categoryParams)
        {
            var assets = await _categoryService.GetCategoriesAsync(categoryParams);
            Response.AddPaginationHeader(assets.Metadata);
            return Ok(assets);
        }
    }
}
