namespace AssetManagement.Application.DTOs.Assignments;

public class ReplyAssignmentRequest
{
    public required int AssignmentId { get; set; }
    public required bool IsAccepted { get; set; }
}