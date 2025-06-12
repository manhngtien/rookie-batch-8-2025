using AssetManagement.Application.DTOs.ReturningRequests;
using AssetManagement.Application.Helpers.Params;
using AssetManagement.Application.Interfaces;
using AssetManagement.Core.Exceptions;
using AssetManagement.Application.Exceptions;
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
        var staffCode = User.GetUserId();
        var returningRequests =
            await _returningRequestService.GetReturningRequestsAsync(staffCode, returningRequestParams);
        
        Response.AddPaginationHeader(returningRequests.Metadata);
        return Ok(returningRequests);
    }


    [HttpPut("{returningRequestId:int}/complete")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ReturningRequestResponse>> CompleteReturningRequest(int returningRequestId, [FromBody] CompleteReturningRequestRequest request)
    {
        if (returningRequestId != request.Id)
        {
            var attributes = new Dictionary<string, object>
            {
                { "returningRequestId", request.Id },
            };
            
            throw new AppException(ErrorCode.INVALID_RETURNING_REQUEST_ID, attributes);
        }

        var staffCode = User.GetUserId();

        await _returningRequestService.CompleteReturningRequestAsync(staffCode, request);
        return NoContent();
    }

    [HttpPut("{returningRequestId:int}/cancel")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CancelReturningRequest(int returningRequestId, [FromForm] CancelReturningRequestRequest request)
    {
        if (returningRequestId != request.Id)
        {
            var attributes = new Dictionary<string, object>
            {
                { "returningRequestId", request.Id },
            };

            throw new AppException(ErrorCode.INVALID_RETURNING_REQUEST_ID, attributes);
        }

        var staffCode = User.GetUserId();

        await _returningRequestService.CancelReturningRequestsAsync(staffCode, request);
        return NoContent();
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateUserReturningRequest([FromBody] CreateUserReturningRequest returningRequest)
    {
        var staffCode = User.GetUserId();

        await _returningRequestService.CreateUserReturningRequestAsync(staffCode, returningRequest);
        return Created();
    }   
}