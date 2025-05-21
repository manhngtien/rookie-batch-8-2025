using AssetManagement.Core.Entities;
using AssetManagement.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AssetManagement.Infrastructure.Repositories;

public class ReturningRequestRepository : IReturningRequestRepository
{
    private readonly AppDbContext _dbContext;

    public ReturningRequestRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public IQueryable<ReturningRequest> GetAllAsync()
    {
        return _dbContext.ReturnRequests
            .Include(r => r.Assignment)
            .ThenInclude(x => x.Asset)
            .Include(r => r.AcceptedByUser)
            .Include(r => r.RequestedByUser);
    }

    public Task<ReturningRequest?> GetByIdAsync<TId>(TId id)
    {
        throw new NotImplementedException();
    }

    public Task<ReturningRequest> CreateAsync(ReturningRequest entity)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(ReturningRequest entity)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(ReturningRequest entity)
    {
        throw new NotImplementedException();
    }

    public Task<ReturningRequest?> GetByIdAsync(int returningRequestId)
    {
        throw new NotImplementedException();
    }
}