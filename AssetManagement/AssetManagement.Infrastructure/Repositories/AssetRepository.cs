using AssetManagement.Core.Entities;
using AssetManagement.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AssetManagement.Infrastructure.Repositories
{
    public class AssetRepository : IAssetRepository
    {
        private readonly AppDbContext _context;

        public AssetRepository(AppDbContext dbContext)
        {
            _context = dbContext;
        }

        public IQueryable<Asset> GetAllAsync()
        {
            return _context.Assets
                .Include(a => a.Category);
        }

        public async Task<Asset?> GetByAssetCodeAsync(string assetCode)
        {
            return await _context.Assets
                .Include(a => a.Category)
                .Include(a => a.Assignments)
                .FirstOrDefaultAsync(a => a.AssetCode == assetCode);
        }
        
        public async Task<int> GetMaxSequenceForCategoryPrefixAsync(string categoryPrefix)
        {
            int prefixLength = categoryPrefix.Length;

            var maxSequenceString = await _context.Assets
                .Where(a => a.AssetCode.StartsWith(categoryPrefix))
                .Select(a => a.AssetCode.Substring(prefixLength))
                .ToListAsync();
        
            int maxSequence = 0;
            foreach (var seqStr in maxSequenceString)
            {
                if (int.TryParse(seqStr, out int currentSeq))
                {
                    if (currentSeq > maxSequence)
                    {
                        maxSequence = currentSeq;
                    }
                }
            }
            
            return maxSequence;
        }
        
        public async Task<Asset> CreateAsync(Asset entity)
        {
            await _context.Assets.AddAsync(entity);
            return entity;
        }

        public async Task UpdateAsync(Asset entity)
        {
            _context.Assets.Update(entity);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(Asset entity)
        {
            _context.Assets.Remove(entity);
            await Task.CompletedTask;
        }
    }
}