using AssetManagement.Core.Entities;
using AssetManagement.Core.Interfaces.Repositories.Base;

namespace AssetManagement.Core.Interfaces.Repositories
{
    public interface IAssetRepository : IBaseRepository<Asset>
    {
        Task<Asset?> GetByIdAsync(string assetCode);
    }
}
