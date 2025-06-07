using System.Diagnostics;
using AssetManagement.Application.DTOs.Reports;
using AssetManagement.Application.Helpers.Params;
using AssetManagement.Application.Interfaces;
using AssetManagement.Application.Mappers;
using AssetManagement.Core.Enums;
using AssetManagement.Core.Interfaces;
using AssetManagement.Core.Shared;
using AssetManagement.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace AssetManagement.Application.Services;

public class ReportService : IReportService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IAssetRepository _assetRepository;

    public ReportService(ICategoryRepository categoryRepository, IAssetRepository assetRepository)
    {
        _categoryRepository = categoryRepository;
        _assetRepository = assetRepository;
    }
    
    public async Task<IList<ReportResponse>> GetReportsAsync(ReportParams reportParams)
    {
        var categories = await _categoryRepository.GetAllAsync().ToListAsync();
        
        var assetCounts = await _assetRepository
            .GetAllAsync()
            .GroupBy(a => new { a.CategoryId, a.State })
            .Select(g => new
            {
                g.Key.CategoryId,
                g.Key.State,
                Count = g.Count()
            })
            .ToListAsync();
        
        var reports = categories.Select(c =>
            {
                var countsForCategory = assetCounts.Where(ac => ac.CategoryId == c.Id).ToList();
                return new Report
                {
                    CategoryName = c.CategoryName,
                    Total = countsForCategory.Sum(ac => ac.Count),
                    TotalAvailable = countsForCategory.FirstOrDefault(ac => ac.State == AssetStatus.Available)?.Count ?? 0,
                    TotalNotAvailable = countsForCategory.FirstOrDefault(ac => ac.State == AssetStatus.Not_Available)?.Count ?? 0,
                    TotalAssigned = countsForCategory.FirstOrDefault(ac => ac.State == AssetStatus.Assigned)?.Count ?? 0,
                    TotalWaitingForRecycling = countsForCategory.FirstOrDefault(ac => ac.State == AssetStatus.Waiting_For_Recycling)?.Count ?? 0,
                    TotalRecycled = countsForCategory.FirstOrDefault(ac => ac.State == AssetStatus.Recycled)?.Count ?? 0
                };
            })
            .AsQueryable()
            .Sort(reportParams.OrderBy)
            .Select(c => c.MapModelToResponse())
            .ToList();

        return reports;
    }
}