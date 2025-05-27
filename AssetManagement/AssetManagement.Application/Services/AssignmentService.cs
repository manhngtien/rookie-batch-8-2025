using AssetManagement.Application.DTOs.Assignments;
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
using Microsoft.Extensions.Logging;

namespace AssetManagement.Application.Services;

public class AssignmentService : IAssignmentService
{
    private readonly IAssignmentRepository _assignmentRepository;
    private readonly IAssetRepository _assetRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AssignmentService> _logger;

    public AssignmentService(
        IAssignmentRepository assignmentRepository,
        IAssetRepository assetRepository,
        IUnitOfWork unitOfWork,
        ILogger<AssignmentService> logger
    )
    {
        _assignmentRepository = assignmentRepository;
        _assetRepository = assetRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<PagedList<AssignmentResponse>> GetAssignmentsAsync(AssignmentParams assignmentParams)
    {
        var query = _assignmentRepository.GetAllAsync()
            .Sort(assignmentParams.OrderBy)
            .Search(assignmentParams.SearchTerm)
            .Filter(assignmentParams.State, assignmentParams.AssignedDate);

        var projectedQuery = query.Select(a => a.MapModelToResponse());

        return await PaginationService.ToPagedList(
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

    public async Task<AssignmentResponse> CreateAssignmentAsync(string adminStaffCode, CreateAssignmentRequest assignmentRequest)
    {
        var asset = await _assetRepository.GetByAssetCodeAsync(assignmentRequest.AssetCode) ?? throw new AppException(ErrorCode.ASSET_NOT_FOUND);

        if (asset.State != AssetStatus.Available)
        {
            var attributes = new Dictionary<string, object> { { "assetCode", asset.AssetCode } };
            throw new AppException(ErrorCode.ASSET_NOT_AVAILABLE, attributes);
        }

        var assignment = new Assignment
        {
            State = AssignmentStatus.Waiting_For_Acceptance,
            AssignedDate = assignmentRequest.AssignedDate,
            Note = assignmentRequest.Note ?? string.Empty,
            AssetCode = assignmentRequest.AssetCode,
            AssignedBy = adminStaffCode,
            AssignedTo = assignmentRequest.StaffCode
        };

        var createdAssignment = await _assignmentRepository.CreateAsync(assignment);

        await _unitOfWork.CommitAsync();

        return createdAssignment.MapModelToResponse();
    }
}