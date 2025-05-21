using AssetManagement.Core.Enums;

namespace AssetManagement.Application.DTOs.Assets;

public class AssetResponse
{
    public required string AssetCode { get; set; }
    public required string AssetName { get; set; }
    public required string Specification { get; set; }
    public required string State { get; set; }
    public ELocation Location { get; set; }
    public DateTime InstalledDate { get; set; }
    public int CategoryId { get; set; }
    public required string CategoryName { get; set; }
}
