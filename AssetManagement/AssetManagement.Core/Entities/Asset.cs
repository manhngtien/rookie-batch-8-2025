using AssetManagement.Core.Enums;

namespace AssetManagement.Core.Entities;

public class Asset
{
    public int Id { get; set; }
    public string AssetCode { get; set; }
    public string AssetName { get; set; }
    public string Specification { get; set; }
    public AssetStatus State { get; set; }
    public ELocation Location { get; set; }
    public DateTime InstalledDate { get; set; }
    
    public int CategoryId { get; set; }
}
