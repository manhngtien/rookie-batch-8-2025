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

        public Task<Category> CreateAsync(Category entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Category entity)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Category> GetAllAsync()
        {
            return _context.Categories;
        }

        public async Task<Category?> GetByIdAsync(int categoryId)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);
        }

        public Task UpdateAsync(Category entity)
        {
            throw new NotImplementedException();
        }
    }
}
