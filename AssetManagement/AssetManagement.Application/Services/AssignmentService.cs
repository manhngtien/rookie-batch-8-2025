using AssetManagement.Application.DTOs.Assignments;
using AssetManagement.Application.Helpers.Params;
using AssetManagement.Application.Interfaces.Assignment;
using AssetManagement.Application.Mappers;
using AssetManagement.Application.Paginations;
using AssetManagement.Core.Exceptions;
using AssetManagement.Core.Interfaces;
using AssetManagement.Infrastructure.Exceptions;
using AssetManagement.Infrastructure.Extensions;
using Microsoft.Extensions.Logging;

namespace AssetManagement.Application.Services;

public class AssignmentService : IAssignmentService
{
    private readonly IAssignmentRepository _assignmentRepository;
    private readonly ILogger<AssignmentService> _logger;

    public AssignmentService(
        IAssignmentRepository assignmentRepository,
        ILogger<AssignmentService> logger)
    {
        _assignmentRepository = assignmentRepository ?? throw new ArgumentNullException(nameof(assignmentRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<PagedList<AssignmentResponse>> GetAssignmentsAsync(AssignmentParams assignmentParams)
    {
        // Only include in-progress assignments (Accepted or Waiting for acceptance)
        var query = _assignmentRepository.GetAllAsync()
            .Sort(assignmentParams.OrderBy)
            .Search(assignmentParams.SearchTerm)
            .Filter(assignmentParams.State, assignmentParams.AssignedDate);

        var projectedQuery = query.Select(a => a.MapModelToResponse());

        return await PaginationService.ToPagedListSync(
            projectedQuery,
            assignmentParams.PageNumber,
            assignmentParams.PageSize
        );
    }

    public async Task<AssignmentResponse> GetAssignmentByIdAsync(int id)
    {
        var assignment = await _assignmentRepository.GetByIdAsync(id);
        if (assignment == null)
        {
            var attributes = new Dictionary<string, object>
            {
                { "assignmentId", id }
            };
            throw new AppException(ErrorCode.ASSIGNMENT_NOT_FOUND, attributes);
        }

        return assignment.MapModelToResponse();
    }
}