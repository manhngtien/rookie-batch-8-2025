using AssetManagement.Api.Controllers.Base;
using AssetManagement.Api.Extensions;
using AssetManagement.Application.DTOs.Assignments;
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
    private readonly IUserService _userService;

    public AssignmentsController(IAssignmentService assignmentService, IUserService userService)
    {
        _assignmentService = assignmentService;
        _userService = userService;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<PagedList<AssignmentResponse>>> GetAssignments([FromQuery] AssignmentParams assignmentParams)
    {
        var result = await _assignmentService.GetAssignmentsAsync(assignmentParams);
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
    public async Task<ActionResult<AssignmentResponse>> CreateAssignment([FromForm] CreateAssignmentRequest assignmentRequest)
    {
        var adminStaffCode = User.GetUserId();

        var adminLocation = await _userService.GetLocationByStaffCodeAsync(adminStaffCode);
        var userLocation = await _userService.GetLocationByStaffCodeAsync(assignmentRequest.StaffCode);

        if (adminLocation == null || userLocation == null || adminLocation != userLocation)
        {
            var attributes = new Dictionary<string, object>
            {
                { "adminStaffCode", adminStaffCode },
                { "userStaffCode", assignmentRequest.StaffCode }
            };
            throw new AppException(ErrorCode.INVALID_LOCATION, attributes);
        }

        var assignment = await _assignmentService.CreateAssignmentAsync(adminStaffCode, assignmentRequest);

        return CreatedAtAction(nameof(GetAssignmentById), new { id = assignment.Id }, assignment);
    }

    [HttpGet("myAssignments")]
    [Authorize]
    public async Task<ActionResult<PagedList<AssignmentResponse>>> GetCurrentUserAssignments([FromQuery] AssignmentParams assignmentParams)
    {
        var staffCode = User.GetUserId();
        var result = await _assignmentService.GetAssignmentsByStaffCodeAsync(staffCode, assignmentParams);
        Response.AddPaginationHeader(result.Metadata);
        return Ok(result);
    }


    //[HttpPut]
    //public IActionResult UpdateAssignments()
    //{
    //    return MethodNotAllowed();
    //}
    
    [HttpDelete("{assignmentId:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteAssignment(int assignmentId)
    {
        var staffCode = User.GetUserId();
        
        await _assignmentService.DeleteAssignmentAsync(staffCode, assignmentId);
        
        return NoContent();
    }
    
    private IActionResult MethodNotAllowed()
    {
        return StatusCode(StatusCodes.Status405MethodNotAllowed);
    }
}