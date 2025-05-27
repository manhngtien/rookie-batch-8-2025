using AssetManagement.Application.DTOs.Assets;
using AssetManagement.Application.Helpers.Params;
using AssetManagement.Application.Paginations;

namespace AssetManagement.Application.Interfaces;

public interface IAssetService
{
    Task<PagedList<AssetResponse>> GetAssetsAsync(AssetParams assetParams);
    Task<AssetResponse> GetAssetByAssetCodeAsync(string assetCode);
    Task<AssetResponse> CreateAssetAsync(string staffCode, CreateAssetRequest createAssetRequest);
}
