namespace AssetManagement.Application.DTOs.Reports;

public class ReportResponse
{
    public required string CategoryName { get; set; }
    public required int Total { get; set; }
    public required int TotalAssigned { get; set; }
    public required int TotalAvailable { get; set; }
    public required int TotalNotAvailable { get; set; }
    public required int TotalWaitingForRecycling { get; set; }
    public required int TotalRecycled { get; set; }
}