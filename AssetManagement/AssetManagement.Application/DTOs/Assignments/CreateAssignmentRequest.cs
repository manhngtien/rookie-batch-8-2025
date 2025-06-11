namespace AssetManagement.Application.DTOs.Assignments;

public class CreateAssignmentRequest
{
    public required string StaffCode { get; set; }
    public required string AssetCode { get; set; }
    public DateTime AssignedDate { get; set; } = DateTime.Now;
    public string? Note { get; set; } = string.Empty;
}
