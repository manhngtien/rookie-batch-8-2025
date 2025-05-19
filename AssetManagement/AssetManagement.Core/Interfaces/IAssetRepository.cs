using AssetManagement.Core.Entities;
using AssetManagement.Core.Interfaces.Base;

namespace AssetManagement.Core.Interfaces
{
    public interface IAssetRepository : IBaseRepository<Asset>
    {
        IQueryable<Asset> GetAllAsync();
        Task<Asset?> GetByIdAsync(Guid id);
    }
}
