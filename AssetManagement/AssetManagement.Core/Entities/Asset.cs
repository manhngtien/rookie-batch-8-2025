using AssetManagement.Core.Enums;

namespace AssetManagement.Core.Entities;

public class Asset
{
    public required string AssetCode { get; set; }
    public required string AssetName { get; set; }
    public required string Specification { get; set; }
    public AssetStatus State { get; set; }
    public ELocation Location { get; set; }
    public DateTime InstalledDate { get; set; }

    // Foreign key   
    public int CategoryId { get; set; }

    // Navigation property
    public virtual Category Category { get; set; } = null!;
}
