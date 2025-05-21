using AssetManagement.Application.DTOs.ReturningRequests;
using AssetManagement.Application.Helpers.Params;
using AssetManagement.Application.Paginations;

namespace AssetManagement.Application.Interfaces;

public interface IReturningRequestService
{
    Task<PagedList<ReturningRequestResponse>> GetReturningRequestsAsync(ReturningRequestParams returningRequestParams);
}