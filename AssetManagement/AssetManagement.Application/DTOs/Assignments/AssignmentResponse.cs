using AssetManagement.Application.DTOs.Users;

namespace AssetManagement.Application.DTOs.Assignments;

public class AssignmentResponse
{
    public int Id { get; set; }
    public required string State { get; set; }
    public required DateOnly AssignedDate { get; set; }
    public required string AssetCode { get; set; }
    public required string AssetName { get; set; }
    public required UserResponse AssignedByUser { get; set; }
    public required UserResponse AssignedToUser { get; set; }
    public required string Note { get; set; }
    public required bool IsReturned { get; set; }
}


