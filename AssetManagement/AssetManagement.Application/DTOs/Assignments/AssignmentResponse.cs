
namespace AssetManagement.Application.DTOs.Assignments;
public class AssignmentResponse
{
    public int Id { get; set; }
    public required string State { get; set; }
    public required DateOnly AssignedDate { get; set; }
    public required string AssetCode { get; set; }
    public required string AssetName { get; set; }
    public required string AssignedBy { get; set; }
    public required string AssignedTo { get; set; }
    public string? Note { get; set; }
}


