using AssetManagement.Core.Entities;
using AssetManagement.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AssetManagement.Infrastructure.Repositories
{
    public class ReturningRequestRepository : IReturningRequestRepository
    {
        private readonly AppDbContext _context;

        public ReturningRequestRepository(AppDbContext dbContext)
        {
            _context = dbContext;
        }

        public IQueryable<ReturningRequest> GetAllAsync()
        {
            return _context.ReturnRequests
                .AsNoTracking()
                .Include(r => r.Assignment)
                .ThenInclude(x => x.Asset)
                .Include(r => r.AcceptedByUser)
                .Include(r => r.RequestedByUser);
        }
        
        public async Task<ReturningRequest?> GetByIdAsync(int returningRequestId)
        {
            return await _context.ReturnRequests
                .Include(r => r.Assignment)
                .ThenInclude(x => x.Asset)
                .Include(r => r.AcceptedByUser)
                .Include(r => r.RequestedByUser)
                .FirstOrDefaultAsync(r => r.Id == returningRequestId);
        }

        public async Task<ReturningRequest> CreateAsync(ReturningRequest entity)
        {
            await _context.ReturnRequests.AddAsync(entity);
            return entity;
        }

        public async Task UpdateAsync(ReturningRequest entity)
        {
            _context.ReturnRequests.Update(entity);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(ReturningRequest entity)
        {
            _context.ReturnRequests.Remove(entity);
            await Task.CompletedTask;
        }
    }
}