﻿using AssetManagement.Core.Entities;
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
                .AsNoTracking()
                .Include(a => a.Category);
        }

        public async Task<Asset?> GetByAssetCodeAsync(string assetCode)
        {
            return await _context.Assets
                .Include(a => a.Category)
                .Include(a => a.Assignments)
                .ThenInclude(a => a.AssignedToUser)
                .Include(a => a.Assignments)
                .ThenInclude(a => a.AssignedByUser)
                .AsSplitQuery()
                .FirstOrDefaultAsync(a => a.AssetCode == assetCode);
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