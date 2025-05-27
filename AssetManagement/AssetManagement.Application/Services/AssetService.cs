using System.Security.Claims;
using AssetManagement.Application.DTOs.Assets;
using AssetManagement.Application.Helpers.Params;
using AssetManagement.Application.Interfaces;
using AssetManagement.Application.Mappers;
using AssetManagement.Application.Paginations;
using AssetManagement.Core.Entities;
using AssetManagement.Core.Enums;
using AssetManagement.Core.Exceptions;
using AssetManagement.Core.Interfaces;
using AssetManagement.Infrastructure.Exceptions;
using AssetManagement.Infrastructure.Extensions;
using Microsoft.AspNetCore.Http;

namespace AssetManagement.Application.Services;

public class AssetService : IAssetService
{
    private readonly IAssetRepository _assetRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AssetService(
        IAssetRepository assetRepository,
        ICategoryRepository categoryRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _assetRepository = assetRepository;
        _categoryRepository = categoryRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
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

    public async Task CreateAssetAsync(string staffCode, CreateAssetRequest createAssetRequest)
    {
        var category = await _categoryRepository.GetByIdAsync(createAssetRequest.CategoryId);
        if (category is null)
        {
            var attributes = new Dictionary<string, object>
            {
                {
                    "categoryId", createAssetRequest.CategoryId
                }
            };

            throw new AppException(ErrorCode.CATEGORY_NOT_FOUND, attributes);
        }
        
        int count = await _assetRepository.GetTotalPrefixByAssetName(createAssetRequest.AssetName) + 1;
        string assetCode = $"{category.Prefix.ToUpper()}{count.ToString().PadLeft(6, '0')}";
        
        var user = await _userRepository.GetByIdAsync(staffCode);
        
        var asset = new Asset
        {
            AssetCode = assetCode,
            AssetName = createAssetRequest.AssetName,
            Specification = createAssetRequest.Specification,
            Location = user.Location,
            InstalledDate = createAssetRequest.InstalledDate,
            CategoryId = createAssetRequest.CategoryId
        };
        
        AssetStatus statusEnumValue;
        if (Enum.TryParse<AssetStatus>(createAssetRequest.State, true, out statusEnumValue))
        {
            asset.State = statusEnumValue;
        }
        else
        {
            var attributes = new Dictionary<string, object>
            {
                {
                    "assetState", createAssetRequest.State
                }
            };
            
            throw new AppException(ErrorCode.ASSET_INVALID_STATE, attributes);
        }
        
        await _assetRepository.CreateAsync(asset);
        await _unitOfWork.CommitAsync();
    }
}
