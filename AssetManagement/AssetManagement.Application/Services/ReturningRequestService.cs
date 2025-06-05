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
    private readonly IAssetRepository _assetRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ReturningRequestService(
        IReturningRequestRepository returningRequestRepository,
        IAssignmentRepository assignmentRepository,
        IUserRepository userRepository,
        IAssetRepository assetRepository,
        IUnitOfWork unitOfWork)
    {
        _returningRequestRepository = returningRequestRepository;
        _assignmentRepository = assignmentRepository;
        _userRepository = userRepository;
        _assetRepository = assetRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<PagedList<ReturningRequestResponse>> GetReturningRequestsAsync(string staffCode, ReturningRequestParams returningRequestParams)
    {
        var staff = await _userRepository.GetByIdAsync(staffCode);
        if (staff is null)
        {
            var attributes = new Dictionary<string, object>
            {
                { "staffCode", staffCode },
            };
            
            throw new AppException(ErrorCode.USER_NOT_FOUND, attributes);
        }
        
        var returningRequests = _returningRequestRepository.GetAllAsync()
            .Where(r => r.Assignment.Asset.Location == staff.Location)
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

    public async Task CreateReturningRequestAsync(string staffCode, CreateAdminReturningRequest request)
    {
        var requestedByUser = await _userRepository.GetByIdAsync(staffCode);
        if (requestedByUser is null)
        {
            var attributes = new Dictionary<string, object>
            {
                { "staffCode", staffCode },
            };
            
            throw new AppException(ErrorCode.USER_NOT_FOUND, attributes);
        }

        var assignment = await _assignmentRepository.GetByIdAsync(request.AssignmentId);
        if (assignment is null)
        {
            var attributes = new Dictionary<string, object>
            {
                { "assignmentId", request.AssignmentId },
            };

            throw new AppException(ErrorCode.ASSIGNMENT_NOT_FOUND, attributes);
        }

        if (requestedByUser.Location != assignment.Asset.Location)
        {
            var attributes = new Dictionary<string, object>
            {
                { "staffLocation", requestedByUser.Location },
                { "assetLocation", assignment.Asset.Location },
                { "assignmentId", request.AssignmentId }
            };

            throw new AppException(ErrorCode.INVALID_LOCATION, attributes);
        }
        
        if (!assignment.State.Equals(AssignmentStatus.Accepted))
        {
            var attributes = new Dictionary<string, object>
            {
                { "assignmentId", request.AssignmentId },
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

        if (assignment.ReturningRequest is not null)
        {
            var attributes = new Dictionary<string, object>
            {
                { "assignmentId", request.AssignmentId },
                { "returningRequestId", assignment.ReturningRequestId!},
            };

            throw new AppException(ErrorCode.ASSIGNMENT_ALREADY_HAD_RETURNING_REQUEST, attributes);
        }
        
        var returningRequest = new ReturningRequest
        {
            AssignmentId = request.AssignmentId,
            State = ReturningRequestStatus.Waiting_For_Returning,
            RequestedBy = staffCode,
        };
        
        await _returningRequestRepository.CreateAsync(returningRequest);
        
        await _unitOfWork.CommitAsync();
    }

    public async Task CancelReturningRequestsAsync(string staffCode, CancelReturningRequestRequest request)
    {
        var admin = await _userRepository.GetByIdAsync(staffCode);
        if (admin is null)
        {
            var attributes = new Dictionary<string, object>
            {
                { "staffCode", staffCode }
            };
            
            throw new AppException(ErrorCode.USER_NOT_FOUND, attributes);
        }
        
        var returningRequest = await _returningRequestRepository.GetByIdAsync(request.Id);
        if (returningRequest is null)
        {
            var attributes = new Dictionary<string, object>
            {
                { "returningRequestId", request.Id }
            };

            throw new AppException(ErrorCode.RETURNING_REQUEST_NOT_FOUND, attributes);
        }
        
        if (admin.Location != returningRequest.Assignment.Asset.Location)
        {
            var attributes = new Dictionary<string, object>
            {
                { "staffLocation", admin.Location },
                { "assetLocation", returningRequest.Assignment.Asset.Location },
                { "returningRequestId", request.Id }
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

        var assignment = await _assignmentRepository.GetByIdAsync(returningRequest.AssignmentId);
        if (assignment is null)
        {
            var attributes = new Dictionary<string, object>
            {
                { "assignmentId", returningRequest.AssignmentId }
            };
            
            throw new AppException(ErrorCode.ASSIGNMENT_NOT_FOUND, attributes);
        }
        
        assignment.ReturningRequestId = null;
        await _returningRequestRepository.DeleteAsync(returningRequest);
        
        await _unitOfWork.CommitAsync();
    }

    public async Task CompleteReturningRequestAsync(string staffCode, CompleteReturningRequestRequest request)
    {
        var currentUserLogin = await _userRepository.GetByIdAsync(staffCode);
        if (currentUserLogin is null)
        {
            var attributes = new Dictionary<string, object>
            {
                { "staffCode", staffCode }
            };
                
            throw new AppException(ErrorCode.USER_NOT_FOUND, attributes);
        }

        var returningRequest = await _returningRequestRepository.GetByIdAsync(request.Id);
        if (returningRequest is null)
        {
            var attributes = new Dictionary<string, object>
            {
                { "returningRequestId", request.Id }
            };
            
            throw new AppException(ErrorCode.RETURNING_REQUEST_NOT_FOUND, attributes);
        }

        if (currentUserLogin.Location != returningRequest.Assignment.Asset.Location)
        {
            var attributes = new Dictionary<string, object>
            {
                { "staffLocation", currentUserLogin.Location },
                { "assetLocation", returningRequest.Assignment.Asset.Location },
                { "returningRequestId", request.Id }
            };

            throw new AppException(ErrorCode.INVALID_LOCATION, attributes);
        }
        
        if (returningRequest.State != ReturningRequestStatus.Waiting_For_Returning)
        {
            var attributes = new Dictionary<string, object>
            {
                {"returningRequestId", request.Id },
                { "state", returningRequest.State.ToString() }
            };
            
            throw new AppException(ErrorCode.INVALID_RETURNING_REQUEST_STATE, attributes);
        }
        
        var assignment = await _assignmentRepository.GetByIdAsync(returningRequest.AssignmentId);
        if (assignment is null)
        {
            var attributes = new Dictionary<string, object>
            {
                { "assignmentId", returningRequest.AssignmentId }
            };
            
            throw new AppException(ErrorCode.ASSIGNMENT_NOT_FOUND,attributes);
        }
        
        returningRequest.State = ReturningRequestStatus.Completed;
        returningRequest.ReturnedDate = DateTime.Now;
        returningRequest.AcceptedBy = currentUserLogin.StaffCode;
        assignment.Asset.State = AssetStatus.Available;
        
        await _unitOfWork.CommitAsync();
    }

    public async Task CreateUserReturningRequestAsync(string staffCode, CreateUserReturningRequest request)
    {
        var requestedByUser = await _userRepository.GetByIdAsync(staffCode);
        if (requestedByUser is null)
        {
            var attributes = new Dictionary<string, object>
            {
                { "staffCode", staffCode }
            };
            
            throw new AppException(ErrorCode.USER_NOT_FOUND, attributes);
        }

        var assignment = await _assignmentRepository.GetByIdAsync(request.AssignmentId);
        if (assignment is null)
        {
            var attributes = new Dictionary<string, object>
            {
                { "assignmentId", request.AssignmentId }
            };

            throw new AppException(ErrorCode.ASSIGNMENT_NOT_FOUND, attributes);
        }

        if (assignment.AssignedTo != requestedByUser.StaffCode)
        {
            var attributes = new Dictionary<string, object>
            {
                { "requestedStaffCode", requestedByUser.StaffCode },
                { "assignedToStaffCode", assignment.AssignedTo }
            };

            throw new AppException(ErrorCode.USER_NOT_ASSIGNED_TO_ASSET, attributes);
        }
        
        if(requestedByUser.Location != assignment.Asset.Location)
        {
            var attributes = new Dictionary<string, object>
            {
                { "staffLocation", requestedByUser.Location },
                { "assetLocation", assignment.Asset.Location },
                { "assignmentId", request.AssignmentId }
            };

            throw new AppException(ErrorCode.INVALID_LOCATION, attributes);
        }
        
        if (assignment.State is not AssignmentStatus.Accepted)
        {
            var attributes = new Dictionary<string, object>
            {
                { "assignmentId", request.AssignmentId },
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
        
        if (assignment.ReturningRequest is not null)
        {
            var attributes = new Dictionary<string, object>
            {
                { "assignmentId", request.AssignmentId },
                { "returningRequestId", assignment.ReturningRequestId!},
            };

            throw new AppException(ErrorCode.ASSIGNMENT_ALREADY_HAD_RETURNING_REQUEST, attributes);
        }
        
        var returningRequest = new ReturningRequest
        {
            AssignmentId = request.AssignmentId,
            State = ReturningRequestStatus.Waiting_For_Returning,
            RequestedBy = requestedByUser.StaffCode
        };

        await _returningRequestRepository.CreateAsync(returningRequest);
        
        await _unitOfWork.CommitAsync();
    }
}