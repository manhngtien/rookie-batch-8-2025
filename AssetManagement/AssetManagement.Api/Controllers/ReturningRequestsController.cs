using AssetManagement.Api.Controllers.Base;
using AssetManagement.Api.Extensions;
using AssetManagement.Application.DTOs.ReturningRequests;
using AssetManagement.Application.Helpers.Params;
using AssetManagement.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssetManagement.Api.Controllers;

public class ReturningRequestsController : BaseApiController
{
    private readonly IReturningRequestService _returningRequestService;

    public ReturningRequestsController(IReturningRequestService returningRequestService)
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
        Response.AddPaginationHeader(returningRequests.Metadata);
        return Ok(returningRequests);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateReturningRequest([FromForm] CreateAdminReturningRequest returningRequest)
    {
        var staffCode = User.GetUserId();

        var savedReturningRequest = await _returningRequestService.CreateReturningRequestAsync(returningRequest, staffCode);
        return CreatedAtAction(nameof(GetReturningRequests), new { id = savedReturningRequest.Id }, savedReturningRequest);
    }
}