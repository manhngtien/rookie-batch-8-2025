using AssetManagement.Application.DTOs.ReturningRequests;
using AssetManagement.Application.Helpers.Params;
using AssetManagement.Application.Paginations;

namespace AssetManagement.Application.Interfaces;

public interface IReturningRequestService
{
    Task<ReturningRequestResponse> CreateReturningRequestAsync(CreateAdminReturningRequest createAdminReturningRequest, string staffCode);
    Task<PagedList<ReturningRequestResponse>> GetReturningRequestsAsync(ReturningRequestParams returningRequestParams);
    Task<ReturningRequestResponse> CompleteReturningRequestAsync(string currentUserStaffCode,CompleteReturningRequestRequest request);
    Task CancelReturningRequestsAsync(string staffCode, CancelReturningRequestRequest request);
}