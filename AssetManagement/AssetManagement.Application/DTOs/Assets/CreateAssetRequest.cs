namespace AssetManagement.Application.DTOs.Assets;

public class CreateAssetRequest
{
    public required string AssetName { get; set; }
    public required int CategoryId { get; set; }
    public required string Specification { get; set; }
    public required DateTime InstalledDate { get; set; }
    public required string State { get; set; }
}