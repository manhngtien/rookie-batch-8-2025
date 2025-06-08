using AssetManagement.Api.Controllers.Base;
using AssetManagement.Application.Helpers.Params;
using AssetManagement.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssetManagement.Api.Controllers;

public class ReportsController : BaseApiController
{
    private readonly IReportService _reportService;

    public ReportsController(IReportService reportService)
    {
        _reportService = reportService;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetReport([FromQuery] ReportParams reportParams)
    {
        var reports = await _reportService.GetReportsAsync(reportParams);

        return Ok(reports);
    }

    [HttpGet("export")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ExportReport()
    {
        var fileBytes = await _reportService.ExportReportsToExcelAsync();
        var fileName = $"AssetManagement_Report_{System.DateTime.UtcNow:yyyyMMdd_HHmmss}.xlsx";

        return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
    }
}