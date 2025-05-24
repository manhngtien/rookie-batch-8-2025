using AssetManagement.Application.DTOs.Assets;
using AssetManagement.Application.Helpers.Params;
using AssetManagement.Application.Interfaces;
using AssetManagement.Application.Mappers;
using AssetManagement.Application.Paginations;
using AssetManagement.Core.Exceptions;
using AssetManagement.Core.Interfaces;
using AssetManagement.Infrastructure.Exceptions;
using AssetManagement.Infrastructure.Extensions;

namespace AssetManagement.Application.Services;

public class AssetService : IAssetService
{
    private readonly IAssetRepository _assetRepository;

    public AssetService(IAssetRepository assetRepository)
    {
        _assetRepository = assetRepository;
    }

    public Task<PagedList<AssetResponse>> GetAssetsAsync(AssetParams assetParams)
    {
        var query = _assetRepository.GetAllAsync()
            .Sort(assetParams.OrderBy)
            .Search(assetParams.SearchTerm)
            .Filter(assetParams.Category, assetParams.State?.ToString());

        var projectedQuery = query.Select(x => x.MapModelToResponse());

        return PaginationService.ToPagedList(
            projectedQuery,
            assetParams.PageNumber,
            assetParams.PageSize
        );
    }

    public async Task<AssetResponse> GetAssetByAssetCodeAsync(string assetCode)
    {
        var asset = await _assetRepository.GetByAssetCodeAsync(assetCode);
        if (asset == null)
        {
            var attributes = new Dictionary<string, object>
            {
                { "assetCode", assetCode }
            };
            throw new AppException(ErrorCode.ASSET_NOT_FOUND, attributes);
        }

        return asset.MapModelToResponse();
    }
}
