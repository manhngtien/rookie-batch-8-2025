using AssetManagement.Application.DTOs.ReturningRequests;
using AssetManagement.Application.Helpers.Params;
using AssetManagement.Application.Paginations;

namespace AssetManagement.Application.Interfaces;

public interface IReturningRequestService
{
    Task<PagedList<ReturningRequestResponse>> GetReturningRequestsAsync(string staffCode, ReturningRequestParams returningRequestParams);
    Task CreateReturningRequestAsync(string staffCode, CreateAdminReturningRequest request);
    Task CompleteReturningRequestAsync(string staffCode,CompleteReturningRequestRequest request);
    Task CancelReturningRequestsAsync(string staffCode, CancelReturningRequestRequest request);
    Task CreateUserReturningRequestAsync(string staffCode, CreateUserReturningRequest request);
}