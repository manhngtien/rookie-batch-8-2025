using AssetManagement.Api.Controllers.Base;
using AssetManagement.Application.DTOs.Categories;
using AssetManagement.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssetManagement.Api.Controllers
{
    public class CategoriesController : BaseApiController
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<CategoryResponse>>> GetCategories()
        {
            var categories = await _categoryService.GetCategoriesAsync();
            
            return Ok(categories);
        }
        
        [HttpGet("{categoryId:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<CategoryResponse?>> GetCategory(int categoryId)
        {
            var category = await _categoryService.GetCategoryByIdAsync(categoryId);
            
            return Ok(category);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<CategoryResponse>> CreateCategory(
            [FromForm] CreateCategoryRequest categoryRequest)
        {
            var category = await _categoryService.CreateCategoryAsync(categoryRequest);
            
            return CreatedAtAction(nameof(GetCategory), new { categoryId = category.Id }, category);
        }
    }
}
