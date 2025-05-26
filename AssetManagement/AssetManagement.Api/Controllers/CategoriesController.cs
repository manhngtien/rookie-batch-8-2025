using AssetManagement.Api.Controllers.Base;
using AssetManagement.Api.Extensions;
using AssetManagement.Application.DTOs.Categories;
using AssetManagement.Application.Helpers.Params;
using AssetManagement.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CategoriesManagement.Api.Controllers
{
    public class CategoriesController : BaseApiController
    {
        private readonly ICategoryService _assetService;

        public CategoriesController(ICategoryService assetService)
        {
            _assetService = assetService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<CategoryResponse>>> GetCategories([FromQuery] CategoryParams assetParams)
        {
            var assets = await _assetService.GetCategoriesAsync(assetParams);
            Response.AddPaginationHeader(assets.Metadata);
            return Ok(assets);
        }
    }
}
