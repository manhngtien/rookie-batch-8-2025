namespace AssetManagement.Core.Interfaces.Repositories;

public interface IUnitOfWork
{
    public Task<int> CommitAsync(CancellationToken cancellationToken);
}