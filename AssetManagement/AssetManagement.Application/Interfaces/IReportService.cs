using AssetManagement.Application.DTOs.Reports;
using AssetManagement.Application.Helpers.Params;

namespace AssetManagement.Application.Interfaces;

public interface IReportService
{
    Task<IList<ReportResponse>> GetReportsAsync(ReportParams reportParams);
}