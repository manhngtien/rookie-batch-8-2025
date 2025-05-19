using AssetManagement.Application.DTOs.Assets;
using AssetManagement.Core.Entities;

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
                State = asset.State.ToString(),
                Location = asset.Location,
                InstalledDate = asset.InstalledDate,
                CategoryId = asset.CategoryId,
                CategoryName = asset.Category.CategoryName,
            };
        }
    }
}
