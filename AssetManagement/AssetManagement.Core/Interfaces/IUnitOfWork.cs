namespace AssetManagement.Core.Interfaces;

public interface IUnitOfWork
{
    public Task<int> CommitAsync(CancellationToken cancellationToken);
}