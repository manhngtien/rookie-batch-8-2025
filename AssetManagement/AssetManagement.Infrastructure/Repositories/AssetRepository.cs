using AssetManagement.Core.Entities;
using AssetManagement.Core.Interfaces;

namespace AssetManagement.Infrastructure.Repositories
{
    public class AssetRepository : IAssetRepository
    {
        public IQueryable<Asset> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Asset?> GetByIdAsync<TId>(TId id)
        {
            throw new NotImplementedException();
        }

        public Task<Asset> CreateAsync(Asset entity)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Asset entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Asset entity)
        {
            throw new NotImplementedException();
        }

        public Task<Asset?> GetByIdAsync(string assetCode)
        {
            throw new NotImplementedException();
        }
    }
}
