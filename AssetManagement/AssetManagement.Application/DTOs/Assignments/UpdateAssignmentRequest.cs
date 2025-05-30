
namespace AssetManagement.Application.DTOs.Assignments;

public class UpdateAssignmentRequest
{
    public required int Id { get; set; }
    public required string StaffCode { get; set; }
    public required string AssetCode { get; set; }
    public DateTime AssignedDate { get; set; }
    public string? Note { get; set; } = string.Empty;
}
