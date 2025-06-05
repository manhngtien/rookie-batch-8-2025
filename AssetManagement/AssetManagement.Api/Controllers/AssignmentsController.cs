using AssetManagement.Api.Controllers.Base;
using AssetManagement.Api.Extensions;
using AssetManagement.Application.DTOs.Assignments;
using AssetManagement.Application.DTOs.ReturningRequests;
using AssetManagement.Application.Helpers.Params;
using AssetManagement.Application.Interfaces;
using AssetManagement.Application.Paginations;
using AssetManagement.Core.Exceptions;
using AssetManagement.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssetManagement.Api.Controllers;

public class AssignmentsController : BaseApiController
{
    private readonly IAssignmentService _assignmentService;
    private readonly IReturningRequestService _returningRequestService;

    public AssignmentsController(IAssignmentService assignmentService, IReturningRequestService returningRequestService)
    {
        _assignmentService = assignmentService;
        _returningRequestService = returningRequestService;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<PagedList<AssignmentResponse>>> GetAssignments([FromQuery] AssignmentParams assignmentParams)
    {
        var staffCode = User.GetUserId();

        var result = await _assignmentService.GetAssignmentsAsync(staffCode, assignmentParams);
        Response.AddPaginationHeader(result.Metadata);
        return Ok(result);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<AssignmentResponse>> GetAssignmentById(int id)
    {
        var assignment = await _assignmentService.GetAssignmentByIdAsync(id);
        return Ok(assignment);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateAssignment([FromForm] CreateAssignmentRequest assignmentRequest)
    {
        var staffCode = User.GetUserId();
        var assignment = await _assignmentService.CreateAssignmentAsync(staffCode, assignmentRequest);

        return CreatedAtAction(nameof(GetAssignmentById), new { id = assignment.Id }, assignment);
    }

    [HttpPut("{assignmentId:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<AssignmentResponse>> UpdateAssignment(int assignmentId, [FromForm] UpdateAssignmentRequest assignmentRequest)
    {
        var staffCode = User.GetUserId();

        if (assignmentId != assignmentRequest.Id)
        {
            var attributes = new Dictionary<string, object>
            {
                {"assignmentId", assignmentId},
                { "id", assignmentRequest.Id }
            };

            throw new AppException(ErrorCode.INVALID_ASSIGNMENT_ID, attributes);
        }

        var updatedAssignment = await _assignmentService.UpdateAssignmentAsync(staffCode, assignmentRequest);

        return Ok(updatedAssignment);
    }

    [HttpPost("{assignmentId:int}/return")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateReturningRequest(int assignmentId, [FromForm] CreateAdminReturningRequest returningRequest)
    {
        if (assignmentId != returningRequest.AssignmentId)
        {
            var attributes = new Dictionary<string, object>
            {
                { "assignmentId", assignmentId },
            };

            throw new AppException(ErrorCode.INVALID_ASSIGNMENT_ID, attributes);
        }

        var staffCode = User.GetUserId();

        await _returningRequestService.CreateReturningRequestAsync(staffCode, returningRequest);
        return Created();
    }

    [HttpGet("my-assignments")]
    [Authorize]
    public async Task<ActionResult<PagedList<AssignmentResponse>>> GetCurrentUserAssignments([FromQuery] AssignmentParams assignmentParams)
    {
        var staffCode = User.GetUserId();
        var result = await _assignmentService.GetAssignmentsByStaffCodeAsync(staffCode, assignmentParams);
        Response.AddPaginationHeader(result.Metadata);
        return Ok(result);
    }

    [HttpDelete("{assignmentId:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteAssignment(int assignmentId)
    {
        var staffCode = User.GetUserId();

        await _assignmentService.DeleteAssignmentAsync(staffCode, assignmentId);

        return NoContent();
    }

    [HttpPut("{assignmentId:int}/reply")]
    [Authorize]
    public async Task<IActionResult> ReplyAssignment(int assignmentId, ReplyAssignmentRequest assignmentRequest)
    {
        var staffCode = User.GetUserId();

        if (assignmentId != assignmentRequest.AssignmentId)
        {
            var attributes = new Dictionary<string, object>
            {
                {"assignmentId", assignmentId},
                {"id", assignmentRequest.AssignmentId},
            };

            throw new AppException(ErrorCode.INVALID_ASSIGNMENT_ID, attributes);
        }

        await _assignmentService.ReplyAssignmentAsync(staffCode, assignmentRequest);

        return NoContent();
    }

    private IActionResult MethodNotAllowed()
    {
        return StatusCode(StatusCodes.Status405MethodNotAllowed);
    }
}