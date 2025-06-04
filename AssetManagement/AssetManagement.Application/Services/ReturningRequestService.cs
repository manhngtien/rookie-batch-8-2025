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
                { "staffCode", staffCode },
            };
            throw new AppException(ErrorCode.USER_NOT_FOUND, attributes);
        }

        var assignment = await _assignmentRepository.GetByIdAsync(createAdminReturningRequest.AssignmentId);
        if (assignment == null)
        {
            var attributes = new Dictionary<string, object>
            {
                { "assignmentId", createAdminReturningRequest.AssignmentId },
            };

            throw new AppException(ErrorCode.ASSIGNMENT_NOT_FOUND, attributes);
        }

        // AC 1
        if (!assignment.State.Equals(AssignmentStatus.Accepted))
        {
            var attributes = new Dictionary<string, object>
            {
                { "assignmentId", createAdminReturningRequest.AssignmentId },
            };

            switch (assignment.State)
            {
                case AssignmentStatus.Waiting_For_Acceptance:
                    throw new AppException(ErrorCode.ASSIGNMENT_NOT_ACCEPTED, attributes);
                case AssignmentStatus.Declined:
                    throw new AppException(ErrorCode.ASSIGNMENT_DECLINED, attributes);
                case AssignmentStatus.Accepted:
                    break;
                default:
                    throw new NotImplementedException();
            }
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

    public async Task CancelReturningRequestsAsync(string staffCode, CancelReturningRequestRequest request)
    {
        var returningRequest = await _returningRequestRepository.GetByIdAsync(request.Id);
        if (returningRequest is null)
        {
            var attributes = new Dictionary<string, object>
            {
                { "returningRequestId", request.Id }
            };

            throw new AppException(ErrorCode.RETURNING_REQUEST_NOT_FOUND, attributes);
        }

        var admin = await _userRepository.GetByIdAsync(staffCode);
        if (admin is null)
        {
            var attributes = new Dictionary<string, object>
            {
                { "staffCode", staffCode }
            };
            throw new AppException(ErrorCode.USER_NOT_FOUND, attributes);
        }

        if (admin.Location != returningRequest.RequestedByUser.Location)
        {
            var attributes = new Dictionary<string, object>
            {
                { "location", returningRequest.RequestedByUser.Location }
            };
            throw new AppException(ErrorCode.INVALID_LOCATION, attributes);
        }

        if (returningRequest.State != ReturningRequestStatus.Waiting_For_Returning)
        {
            var attributes = new Dictionary<string, object>
            {
                { "state", returningRequest.State }
            };
            throw new AppException(ErrorCode.INVALID_RETURNING_REQUEST_STATE, attributes);
        }

        await _returningRequestRepository.DeleteAsync(returningRequest);

        await _unitOfWork.CommitAsync();
    }
}