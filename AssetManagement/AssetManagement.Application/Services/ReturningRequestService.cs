using AssetManagement.Application.DTOs.ReturningRequests;
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

namespace AssetManagement.Application.Services;

public class ReturningRequestService : IReturningRequestService
{
    private readonly IReturningRequestRepository _returningRequestRepository;
    private readonly IAssignmentRepository _assignmentRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ReturningRequestService(
        IReturningRequestRepository returningRequestRepository,
        IAssignmentRepository assignmentRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _returningRequestRepository = returningRequestRepository;
        _assignmentRepository = assignmentRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<PagedList<ReturningRequestResponse>> GetReturningRequestsAsync(ReturningRequestParams returningRequestParams)
    {
        var returningRequests = _returningRequestRepository.GetAllAsync()
            .Sort(returningRequestParams.OrderBy)
            .Search(returningRequestParams.SearchTerm)
            .Filter(returningRequestParams.State, returningRequestParams.ReturnedDate);

        var returningRequestDto = returningRequests.Select(x => x.MapModelToResponse());

        return await PaginationService.ToPagedList(
            returningRequestDto,
            returningRequestParams.PageNumber,
            returningRequestParams.PageSize
        );
    }

    public async Task<ReturningRequestResponse> CreateReturningRequestAsync(CreateAdminReturningRequest createAdminReturningRequest, string staffCode)
    {
        var requestedByUser = await _userRepository.GetByIdAsync(staffCode);
        if (requestedByUser == null)
        {
            var attributes = new Dictionary<string, object>
            {
                { "StaffCode", staffCode },
            };
            throw new AppException(ErrorCode.USER_NOT_FOUND, attributes);
        }

        var assignment = await _assignmentRepository.GetByIdAsync(createAdminReturningRequest.AssignmentId);
        if(assignment == null)
        {
            var attributes = new Dictionary<string, object>
            {
                { "AssignmentId", createAdminReturningRequest.AssignmentId },
            };
            throw new AppException(ErrorCode.ASSIGNMENT_NOT_FOUND, attributes);
        }

        // AC 1
        if (assignment.State.Equals(AssignmentStatus.Waiting_For_Acceptance))
        {
            var attributes = new Dictionary<string, object>
            {
                { "AssignmentId", createAdminReturningRequest.AssignmentId },
            };
            throw new AppException(ErrorCode.ASSIGNMENT_NOT_ACCEPTED, attributes);
        }

        var returningRequest = new ReturningRequest
        {
            AssignmentId = createAdminReturningRequest.AssignmentId,
            State = ReturningRequestStatus.Waiting_For_Returning,
            RequestedByUser = requestedByUser,
        };

        var savedRfr = await _returningRequestRepository.CreateAsync(returningRequest);

        await _unitOfWork.CommitAsync();

        return savedRfr.MapModelToResponse();
    }
}