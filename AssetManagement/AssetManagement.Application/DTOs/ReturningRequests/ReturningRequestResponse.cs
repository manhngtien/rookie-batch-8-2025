namespace AssetManagement.Application.DTOs.ReturningRequests;

public class ReturningRequestResponse
{
    public required int Id { get; set; }
    public required string AssetCode { get; set; }
    public required string AssetName { get; set; }
    public required DateOnly AssignedDate { get; set; }
    public required string RequestedBy { get; set; }
    public string? AcceptedBy { get; set; } = "";
    public DateOnly? ReturnedDate { get; set; } = null;
    public required string State { get; set; }
}