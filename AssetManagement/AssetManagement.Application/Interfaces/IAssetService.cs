﻿using AssetManagement.Application.DTOs.Assets;
using AssetManagement.Application.Helpers.Params;
using AssetManagement.Application.Paginations;

namespace AssetManagement.Application.Interfaces;

public interface IAssetService
{
    Task<PagedList<AssetResponse>> GetAssetsAsync(string staffCode, AssetParams assetParams);
    Task<AssetResponse?> GetAssetByAssetCodeAsync(string assetCode, string staffCode);
    Task<AssetResponse> CreateAssetAsync(string staffCode, CreateAssetRequest createAssetRequest);
    Task<PagedList<AssetResponse>> GetUserAssetsAsync(AssetParams assetParams, string staffCode);
    Task<AssetResponse> UpdateAssetAsync(string assetCode, UpdateAssetRequest updateAssetRequest);
    Task DeleteAssetAsync(string assetCode);
}
