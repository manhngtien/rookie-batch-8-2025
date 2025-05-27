using AssetManagement.Core.Entities;
using AssetManagement.Core.Interfaces.Base;

namespace AssetManagement.Core.Interfaces
{
    public interface IAssetRepository : IBaseRepository<Asset>
    {
        Task<Asset?> GetByAssetCodeAsync(string assetCode);
        Task<int> GetMaxSequenceForCategoryPrefixAsync(string categoryPrefix);
    }
}
