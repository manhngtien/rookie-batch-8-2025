using AssetManagement.Application.DTOs.Assets;
using AssetManagement.Core.Entities;
using AssetManagement.Core.Enums;

namespace AssetManagement.Application.Mappers
{
    public static class AssetMapper
    {
        public static AssetResponse MapModelToResponse(this Asset asset)
        {
            return new AssetResponse
            {
                AssetCode = asset.AssetCode,
                AssetName = asset.AssetName,
                Specification = asset.Specification,
                State = Enum.GetName<AssetStatus>(asset.State)!,
                Location = asset.Location,
                InstalledDate = DateOnly.FromDateTime(asset.InstalledDate),
                Category = asset.Category.MapModelToResponse(),
                Assignments = asset.Assignments.Select(a => a.MapModelToResponse()).ToList()
            };
        }
    }
}
