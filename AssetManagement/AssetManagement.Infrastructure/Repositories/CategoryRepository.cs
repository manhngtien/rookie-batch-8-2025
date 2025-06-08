using AssetManagement.Core.Entities;
using AssetManagement.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AssetManagement.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext dbContext)
        {
            _context = dbContext;
        }

        public async Task<Category> CreateAsync(Category entity)
        {
            await _context.Categories.AddAsync(entity);
            return entity;
        }

        public Task DeleteAsync(Category entity)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Category> GetAllAsync()
        {
            return _context.Categories.AsNoTracking();
        }

        public async Task<Category?> GetByIdAsync(int categoryId)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);
        }

        public async Task<bool> FindCategoryByName(string categoryName)
        {
            return await _context.Categories.AnyAsync(c => c.CategoryName == categoryName);
        }

        public async Task<bool> FindCategoryByPrefix(string prefix)
        {
            return await _context.Categories.AnyAsync(c => c.Prefix == prefix);
        }

        public Task UpdateAsync(Category entity)
        {
            throw new NotImplementedException();
        }
    }
}
