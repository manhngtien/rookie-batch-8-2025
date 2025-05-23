using AssetManagement.Api.Controllers.Base;
using AssetManagement.Api.Extensions;
using AssetManagement.Application.DTOs.Assignments;
using AssetManagement.Application.Helpers.Params;
using AssetManagement.Application.Interfaces.Assignment;
using AssetManagement.Application.Paginations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssetManagement.Api.Controllers;

public class AssignmentsController : BaseApiController
{
    private readonly IAssignmentService _service;

    public AssignmentsController(IAssignmentService service)
    {
        _service = service;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<PagedList<AssignmentResponse>>> GetAssignments([FromQuery] AssignmentParams assignmentParams)
    {
        var result = await _service.GetAssignmentsAsync(assignmentParams);
        Response.AddPaginationHeader(result.Metadata);
        return Ok(result);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<AssignmentResponse>> GetAssignmentById(int id)
    {
        var assignment = await _service.GetAssignmentByIdAsync(id);
        return Ok(assignment);
    }


    //[HttpPut]
    //public IActionResult UpdateAssignments()
    //{
    //    return MethodNotAllowed();
    //}


    //[HttpDelete]
    //public IActionResult DeleteAssignments()
    //{
    //    return MethodNotAllowed();
    //}


    private IActionResult MethodNotAllowed()
    {
        return StatusCode(StatusCodes.Status405MethodNotAllowed);
    }
}