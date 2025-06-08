using AssetManagement.Application.DTOs.Reports;
using AssetManagement.Application.Helpers.Params;
using AssetManagement.Application.Interfaces;
using AssetManagement.Application.Mappers;
using AssetManagement.Core.Enums;
using AssetManagement.Core.Interfaces;
using AssetManagement.Core.Shared;
using AssetManagement.Infrastructure.Extensions;
using ClosedXML.Excel;
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

    public async Task<byte[]> ExportReportsToExcelAsync()
    {
        var reports = await _categoryRepository
            .GetAllAsync()
            .Select(c => new Report
            {
                CategoryName = c.CategoryName,
                Total = c.Total,
                TotalAvailable = c.Assets.Count(a => a.State == AssetStatus.Available),
                TotalNotAvailable = c.Assets.Count(a => a.State == AssetStatus.Not_Available),
                TotalAssigned = c.Assets.Count(a => a.State == AssetStatus.Assigned),
                TotalWaitingForRecycling = c.Assets.Count(a => a.State == AssetStatus.Waiting_For_Recycling),
                TotalRecycled = c.Assets.Count(a => a.State == AssetStatus.Recycled)
            })
            .ToListAsync();

        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Asset Report");

        // Header
        worksheet.Cell(1, 1).Value = "Category";
        worksheet.Cell(1, 2).Value = "Total";
        worksheet.Cell(1, 3).Value = "Available";
        worksheet.Cell(1, 4).Value = "Not Available";
        worksheet.Cell(1, 5).Value = "Assigned";
        worksheet.Cell(1, 6).Value = "Waiting for Recycling";
        worksheet.Cell(1, 7).Value = "Recycled";

        // Style header
        var headerRow = worksheet.Row(1);
        headerRow.Style.Font.Bold = true;
        headerRow.Style.Fill.BackgroundColor = XLColor.LightGray;

        // Data
        int currentRow = 2;
        foreach (var report in reports)
        {
            worksheet.Cell(currentRow, 1).Value = report.CategoryName;
            worksheet.Cell(currentRow, 2).Value = report.Total;
            worksheet.Cell(currentRow, 3).Value = report.TotalAvailable;
            worksheet.Cell(currentRow, 4).Value = report.TotalNotAvailable;
            worksheet.Cell(currentRow, 5).Value = report.TotalAssigned;
            worksheet.Cell(currentRow, 6).Value = report.TotalWaitingForRecycling;
            worksheet.Cell(currentRow, 7).Value = report.TotalRecycled;

            currentRow++;
        }

        worksheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }
}