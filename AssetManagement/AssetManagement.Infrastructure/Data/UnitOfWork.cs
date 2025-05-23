using AssetManagement.Core.Interfaces;

namespace AssetManagement.Infrastructure.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _dbContext;

    public UnitOfWork(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> CommitAsync()
    {
        return await _dbContext.SaveChangesAsync();
    }

}