using AssetManagement.Application.DTOs.Assignments;
using AssetManagement.Application.DTOs.Categories;
using AssetManagement.Core.Enums;

namespace AssetManagement.Application.DTOs.Assets;

public class AssetResponse
{
    public required string AssetCode { get; set; }
    public required string AssetName { get; set; }
    public required string Specification { get; set; }
    public required string State { get; set; }
    public required ELocation Location { get; set; }
    public required DateOnly InstalledDate { get; set; }
    public required CategoryResponse Category { get; set; }
    public List<AssignmentResponse> Assignments { get; set; } = [];
}
