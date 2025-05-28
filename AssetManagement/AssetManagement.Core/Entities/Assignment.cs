using AssetManagement.Core.Enums;

namespace AssetManagement.Core.Entities;

public class Assignment
{
    public int Id { get; set; }
    public AssignmentStatus State { get; set; }
    public required DateTime AssignedDate { get; set; }
    public string Note { get; set; } = string.Empty;

    // Foreign keys
    public required string AssetCode { get; set; } 
    public required string AssignedBy { get; set; }
    public required string AssignedTo { get; set; }
    public int? ReturningRequestId { get; set; }

    // Navigation properties
    public virtual Asset Asset { get; set; } = null!;
    public virtual User AssignedByUser { get; set; } = null!;
    public virtual User AssignedToUser { get; set; } = null!;
    public virtual ReturningRequest? ReturningRequest { get; set; }
}
