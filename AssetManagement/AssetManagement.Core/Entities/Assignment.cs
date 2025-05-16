using AssetManagement.Core.Enums;

namespace AssetManagement.Core.Entities;

public class Assignment
{
    public int Id { get; set; }
    public string AssetCode { get; set; }
    public AssignmentStatus State { get; set; }
    public DateTime AssignedDate { get; set; }
    public string Note { get; set; }
    public Guid AssignBy { get; set; }
    public Guid AssignedTo { get; set; }
}
