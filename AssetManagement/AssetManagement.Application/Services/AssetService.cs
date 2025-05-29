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
using AssetManagement.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

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
        IUnitOfWork unitOfWork
    )
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

    public async Task<AssetResponse> CreateAssetAsync(string staffCode, CreateAssetRequest createAssetRequest)
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

        int nextSequence = category.Total + 1;
        int length = 8 - category.Prefix.Length;
        string assetCode = $"{category.Prefix.ToUpper()}{nextSequence.ToString().PadLeft(length, '0')}";
        
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
            if (statusEnumValue != AssetStatus.Available && statusEnumValue != AssetStatus.Not_Available)
            {
                var attributes = new Dictionary<string, object>
                {
                    {
                        "assetState", createAssetRequest.State
                    }
                };
                
                throw new AppException(ErrorCode.ASSET_INVALID_STATE, attributes);
            }

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
        
        var createdAsset = await _assetRepository.CreateAsync(asset);
        category.Total++;
        
        await _unitOfWork.CommitAsync();

        return createdAsset.MapModelToResponse();
    }

    public async Task<PagedList<AssetResponse>> GetUserAssetsAsync(AssetParams assetParams, string staffCode)
    {
        var query = _assetRepository.GetAllAsync()
            .Where(asset => asset.Assignments.Any(a => a.AssignedTo == staffCode))
            .Sort(assetParams.OrderBy)
            .Search(assetParams.SearchTerm)
            .Filter(assetParams.Category, assetParams.State?.ToString());

        var projectedQuery = query.Select(x => x.MapModelToResponse());

        return await PaginationService.ToPagedList(
            projectedQuery,
            assetParams.PageNumber,
            assetParams.PageSize
        );
    }

    public async Task<AssetResponse> UpdateAssetAsync(string assetCode, UpdateAssetRequest updateAssetRequest)
    {
        // Fetch the asset
        var asset = await _assetRepository.GetByAssetCodeAsync(assetCode);
        if (asset == null)
        {
            var attributes = new Dictionary<string, object>
            {
                { "assetCode", assetCode }
            };
            throw new AppException(ErrorCode.ASSET_NOT_FOUND, attributes);
        }

        // AC1: Cannot edit assets in "Assigned" state
        if (asset.State == AssetStatus.Assigned)
        {
            var attributes = new Dictionary<string, object>
            {
                { "assetCode", assetCode },
                { "state", asset.State.ToString() }
            };
            throw new AppException(ErrorCode.ASSET_CANNOT_BE_EDITED, attributes);
        }

        // Validate the state
        if (!Enum.TryParse<AssetStatus>(updateAssetRequest.State, true, out var newState))
        {
            var attributes = new Dictionary<string, object>
            {
                { "state", updateAssetRequest.State }
            };
            throw new AppException(ErrorCode.INVALID_ASSET_STATE, attributes);
        }

        // Validate InstalledDate (not in the future relative to today, May 27, 2025)
        var today = DateTime.UtcNow;
        if (updateAssetRequest.InstalledDate > today)
        {
            var attributes = new Dictionary<string, object>
            {
                { "installedDate", updateAssetRequest.InstalledDate.ToString("yyyy-MM-dd") }
            };
            throw new AppException(ErrorCode.INVALID_DATE, attributes);
        }

        // Update the asset fields
        asset.AssetName = updateAssetRequest.AssetName;
        asset.Specification = updateAssetRequest.Specification;
        asset.InstalledDate = updateAssetRequest.InstalledDate.Date;
        asset.State = newState;

        try
        {
            await _assetRepository.UpdateAsync(asset);
            await _unitOfWork.CommitAsync();
        }
        catch (DbUpdateException)
        {
            throw new AppException(ErrorCode.SAVE_ERROR, new Dictionary<string, object>());
        }

        return asset.MapModelToResponse();
    }

    public async Task DeleteAssetAsync(string assetCode)
    {
        // Fetch the asset
        var asset = await _assetRepository.GetByAssetCodeAsync(assetCode);
        if (asset == null)
        {
            var attributes = new Dictionary<string, object>
            {
                { "assetCode", assetCode }
            };
            throw new AppException(ErrorCode.ASSET_NOT_FOUND, attributes);
        }

        // Check if the asset has any historical assignments
        if (asset.Assignments != null && asset.Assignments.Any())
        {
            var attributes = new Dictionary<string, object>
            {
                { "assetCode", assetCode }
            };
            throw new AppException(ErrorCode.ASSET_HAS_HISTORICAL_ASSIGNMENTS, attributes);

        }

        // Prevent deletion if the asset is in "Assigned" state
        if (asset.State == AssetStatus.Assigned)
        {
            var attributes = new Dictionary<string, object>
            {
                { "assetCode", assetCode },
                { "state", asset.State.ToString() }
            };
            throw new AppException(ErrorCode.ASSET_CANNOT_BE_DELETED, attributes);
        }

        try
        {
            await _assetRepository.DeleteAsync(asset);
            await _unitOfWork.CommitAsync(); // Commit changes using UnitOfWork
        }
        catch (DbUpdateException)
        {
            throw new AppException(ErrorCode.SAVE_ERROR, new Dictionary<string, object>());
        }
    }
}
