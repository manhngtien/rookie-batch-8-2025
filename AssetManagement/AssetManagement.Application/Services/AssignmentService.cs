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
using Microsoft.Identity.Client;

namespace AssetManagement.Application.Services;

public class AssignmentService : IAssignmentService
{
    private readonly IAssignmentRepository _assignmentRepository;
    private readonly IAssetRepository _assetRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AssignmentService> _logger;

    public AssignmentService(
        IAssignmentRepository assignmentRepository,
        IAssetRepository assetRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        ILogger<AssignmentService> logger
    )
    {
        _assignmentRepository = assignmentRepository;
        _assetRepository = assetRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<PagedList<AssignmentResponse>> GetAssignmentsAsync(AssignmentParams assignmentParams)
    {
        var query = _assignmentRepository.GetAllAsync()
            .Sort(assignmentParams.OrderBy)
            .Search(assignmentParams.SearchTerm)
            .Filter(assignmentParams.State, assignmentParams.AssignedDate)
            .Where(a => a.ReturningRequest == null || a.ReturningRequest.State != ReturningRequestStatus.Completed);

        var projectedQuery = query.Select(a => a.MapModelToResponse());

        return await PaginationService.ToPagedList(
            projectedQuery,
            assignmentParams.PageNumber,
            assignmentParams.PageSize
        );
    }

    public async Task<PagedList<AssignmentResponse>> GetAssignmentsByStaffCodeAsync(string staffCode, AssignmentParams assignmentParams)
    {
        var query = _assignmentRepository.GetAllAsync()
            .Sort(assignmentParams.OrderBy)
            .Where(a => a.AssignedTo == staffCode && a.AssignedDate <= DateTime.UtcNow)
            .Where(a => a.ReturningRequest == null || a.ReturningRequest.State != ReturningRequestStatus.Completed);

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

    public async Task<AssignmentResponse> UpdateAssignmentAsync(int id, string staffCode, UpdateAssignmentRequest assignmentRequest)
    {
        var staff = await _userRepository.GetByIdAsync(staffCode);
        if (staff is null)
        {
            var attributes = new Dictionary<string, object>
            {
                { "staffCode", staffCode }
            };

            throw new AppException(ErrorCode.USER_NOT_FOUND, attributes);
        }

        var asset = await _assetRepository.GetByAssetCodeAsync(assignmentRequest.AssetCode);
        if (asset is null)
        {
            var attributes = new Dictionary<string, object> { { "assetCode", assignmentRequest.AssetCode } };
            throw new AppException(ErrorCode.ASSET_NOT_FOUND, attributes);
        }

        if (asset.State != AssetStatus.Available)
        {
            var attributes = new Dictionary<string, object> { { "assetCode", asset.AssetCode } };
            throw new AppException(ErrorCode.ASSET_NOT_AVAILABLE, attributes);
        }

        var assignment = await _assignmentRepository.GetByIdAsync(id);
        if (assignment == null)
        {
            var attributes = new Dictionary<string, object>
            {
                { "assignmentId", id }
            };
            throw new AppException(ErrorCode.ASSIGNMENT_NOT_FOUND, attributes);
        }

        if (staff.Location != asset.Location)
        {
            var attributes = new Dictionary<string, object>
            {
                { "location", asset.Location.ToString() }
            };

            throw new AppException(ErrorCode.INVALID_LOCATION, attributes);
        }

        if (assignment.State != AssignmentStatus.Waiting_For_Acceptance)
        {
            var attributes = new Dictionary<string, object>
            {
                { "assetCode", assignment.AssetCode }
            };
            throw new AppException(ErrorCode.ASSIGNMENT_STATE_IS_NOT_WAITING_FOR_ACCEPTANCE, attributes);
        }

        if (assignment.AssetCode != assignmentRequest.AssetCode)
        {
            if (assignment.Asset.State is not AssetStatus.Assigned)
            {
                var attributes = new Dictionary<string, object>
                {
                    { "assetCode", assignment.AssetCode }
                };
                throw new AppException(ErrorCode.ASSET_INVALID_STATE, attributes);
            }
            assignment.Asset.State = AssetStatus.Available;
            asset.State = AssetStatus.Assigned;
        }
        var newAssignedTo = await _userRepository.GetByIdAsync(assignmentRequest.StaffCode);
        if (newAssignedTo is null)
        {
            var attributes = new Dictionary<string, object>
            {
                { "staffCode", staffCode }
            };

            throw new AppException(ErrorCode.USER_NOT_FOUND, attributes);
        }
        assignment.AssignedTo = assignmentRequest.StaffCode;
        assignment.AssignedDate = assignmentRequest.AssignedDate;
        assignment.Note = assignmentRequest.Note ?? string.Empty;
        await _assignmentRepository.UpdateAsync(assignment);
        await _unitOfWork.CommitAsync();

        return assignment.MapModelToResponse();
    }

    public async Task DeleteAssignmentAsync(string staffCode, int assignmentId)
    {
        var staff = await _userRepository.GetByIdAsync(staffCode);
        if (staff is null)
        {
            var attributes = new Dictionary<string, object>
            {
                { "staffCode", staffCode }
            };

            throw new AppException(ErrorCode.USER_NOT_FOUND, attributes);
        }

        var assignment = await _assignmentRepository.GetByIdAsync(assignmentId);
        if (assignment is null)
        {
            var attributes = new Dictionary<string, object>
            {
                { "assignmentId", assignmentId }
            };

            throw new AppException(ErrorCode.ASSIGNMENT_NOT_FOUND, attributes);
        }

        var asset = await _assetRepository.GetByAssetCodeAsync(assignment.AssetCode);
        if (asset is null)
        {
            var attributes = new Dictionary<string, object>
            {
                { "assetCode", assignment.AssetCode }
            };

            throw new AppException(ErrorCode.ASSET_NOT_FOUND, attributes);
        }


        if (asset.State is not AssetStatus.Available)
        {
            var attributes = new Dictionary<string, object>
            {
                { "assetCode", asset.AssetCode }
            };

            throw new AppException(ErrorCode.ASSET_INVALID_STATE, attributes);
        }


        if (staff.Location != asset.Location)
        {
            var attributes = new Dictionary<string, object>
            {
                { "assetCode", assignment.AssetCode },
                { "location", asset.Location.ToString() }
            };

            throw new AppException(ErrorCode.INVALID_LOCATION, attributes);
        }

        if (assignment.State is AssignmentStatus.Accepted)
        {
            var attributes = new Dictionary<string, object>
            {
                { "assignmentId", assignmentId }
            };

            throw new AppException(ErrorCode.ASSIGNMENT_ALREADY_ACCEPTED, attributes);
        }

        await _assignmentRepository.DeleteAsync(assignment);

        await _unitOfWork.CommitAsync();
    }
}