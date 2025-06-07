using AssetManagement.Core.Entities;
using AssetManagement.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AssetManagement.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> CreateAsync(User entity)
        {
            await _context.Staffs.AddAsync(entity);
            return entity;
        }

        public async Task DeleteAsync(User entity)
        {
            _context.Staffs.Remove(entity);
            await Task.CompletedTask;
        }

        public IQueryable<User> GetAllAsync()
        {
            return _context.Staffs.AsNoTracking();
        }

        public async Task<User?> GetByIdAsync(string userId)
        {
            return await _context.Staffs
                .FirstOrDefaultAsync(x => x.StaffCode == userId);
        }

        public async Task UpdateAsync(User entity)
        {
            _context.Staffs.Update(entity);
            await Task.CompletedTask;
        }
    }
}