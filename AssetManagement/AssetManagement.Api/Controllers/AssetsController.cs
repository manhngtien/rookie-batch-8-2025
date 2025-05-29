using AssetManagement.Api.Controllers.Base;
using AssetManagement.Api.Extensions;
using AssetManagement.Application.DTOs.Assets;
using AssetManagement.Application.Helpers.Params;
using AssetManagement.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssetManagement.Api.Controllers
{
    public class AssetsController : BaseApiController
    {
        private readonly IAssetService _assetService;
        public AssetsController(IAssetService assetService)
        {
            _assetService = assetService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<AssetResponse>>> GetAssets([FromQuery] AssetParams assetParams)
        {
            var assets = await _assetService.GetAssetsAsync(assetParams);
            Response.AddPaginationHeader(assets.Metadata);
            return Ok(assets);
        }

        [HttpGet("{assetCode}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<AssetResponse?>> GetAsset(string assetCode)
        {
            var asset = await _assetService.GetAssetByAssetCodeAsync(assetCode);
            return Ok(asset);
        }
        
        [HttpPost()]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<AssetResponse>> CreateAsset([FromForm] CreateAssetRequest assetRequest)
        {
            var staffCode = User.GetUserId();
            var asset = await _assetService.CreateAssetAsync(staffCode, assetRequest);

            return CreatedAtAction(nameof(GetAsset), new { assetCode = asset.AssetCode }, asset);
        }


        [HttpDelete("{assetCode}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAssets(string assetCode)
        {
            await _assetService.DeleteAssetAsync(assetCode);
            return NoContent();
        }
    }
}
