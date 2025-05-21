using AssetManagement.Core.Enums;

namespace AssetManagement.Core.Entities;
public class ReturningRequest
{
    public int Id { get; set; }
    public ReturningRequestStatus State { get; set; }
    public DateTime? ReturnedDate { get; set; }
    
    // Foreign keys
    public string RequestedBy { get; set; } = null!;
    public string? AcceptedBy { get; set; }
    public int AssignmentId { get; set; }
    
    // Navigation properties
    public virtual User RequestedByUser { get; set; } = null!;
    public virtual User AcceptedByUser { get; set; }
    public virtual Assignment Assignment { get; set; } = null!;
}