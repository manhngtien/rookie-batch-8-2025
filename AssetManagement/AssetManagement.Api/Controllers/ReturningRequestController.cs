using AssetManagement.Api.Controllers.Base;
using AssetManagement.Application.DTOs.ReturningRequests;
using AssetManagement.Application.Helpers.Params;
using AssetManagement.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssetManagement.Api.Controllers;

public class ReturningRequestController : BaseApiController
{
    private readonly IReturningRequestService _returningRequestService;

    public ReturningRequestController(IReturningRequestService returningRequestService)
    {
        _returningRequestService = returningRequestService;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<ReturningRequestResponse>>> GetReturningRequests(
        [FromQuery] ReturningRequestParams returningRequestParams)
    {
        var returningRequests =
            await _returningRequestService.GetReturningRequestsAsync(returningRequestParams);
        return Ok(returningRequests);
    }
}