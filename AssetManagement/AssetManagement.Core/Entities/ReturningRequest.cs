using AssetManagement.Core.Enums;

namespace AssetManagement.Core.Entities;
public class ReturningRequest
{
    public Guid Id { get; set; }
    public ReturningRequestStatus State { get; set; }
    public DateTime ReturnedDate { get; set; }
    
    // Foreign keys
    public Guid RequestedBy { get; set; }
    public Guid AcceptedBy { get; set; }
    
    // Navigation properties
    public virtual User RequestedByUser { get; set; } = null!;
    public virtual User AcceptedByUser { get; set; } = null!;
}
