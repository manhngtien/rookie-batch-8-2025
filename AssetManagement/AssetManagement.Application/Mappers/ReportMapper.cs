using AssetManagement.Application.DTOs.Reports;
using AssetManagement.Core.Shared;

namespace AssetManagement.Application.Mappers;

public static class ReportMapper
{
    public static ReportResponse MapModelToResponse(this Report report)
    {
        return new ReportResponse
        {
            CategoryName = report.CategoryName,
            Total = report.Total,
            TotalAvailable = report.TotalAvailable,
            TotalNotAvailable = report.TotalNotAvailable,
            TotalAssigned = report.TotalAssigned,
            TotalWaitingForRecycling = report.TotalWaitingForRecycling,
            TotalRecycled = report.TotalRecycled,
        };
    }
}