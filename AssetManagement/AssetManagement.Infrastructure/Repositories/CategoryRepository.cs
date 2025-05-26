using AssetManagement.Core.Entities;
using AssetManagement.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public Task<Category?> GetByIdAsync(int categoryId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Category entity)
        {
            throw new NotImplementedException();
        }
    }
}
