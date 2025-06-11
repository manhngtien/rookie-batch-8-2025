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
            var staffCode = User.GetUserId();

            var assets = await _assetService.GetAssetsAsync(staffCode, assetParams);
            Response.AddPaginationHeader(assets.Metadata);
            return Ok(assets);
        }

        [HttpGet("{assetCode}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<AssetResponse?>> GetAsset(string assetCode)
        {
            var staffCode = User.GetUserId();

            var asset = await _assetService.GetAssetByAssetCodeAsync(assetCode, staffCode);
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

        [HttpPut("{assetCode}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<AssetResponse>> UpdateAssets(string assetCode, [FromBody] UpdateAssetRequest updateAssetRequest)
        {
            var updatedAsset = await _assetService.UpdateAssetAsync(assetCode, updateAssetRequest);
            return Ok(updatedAsset);
        }
    }
}
