using AssetManagement.Application.DTOs.ReturningRequests;
using AssetManagement.Application.Helpers.Params;
using AssetManagement.Application.Interfaces;
using AssetManagement.Application.Mappers;
using AssetManagement.Application.Paginations;
using AssetManagement.Core.Interfaces;
using AssetManagement.Infrastructure.Extensions;

namespace AssetManagement.Application.Services;

public class ReturningRequestService : IReturningRequestService
{
    private readonly IReturningRequestRepository _returningRequestRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ReturningRequestService(IReturningRequestRepository returningRequestRepository, IUnitOfWork unitOfWork)
    {
        _returningRequestRepository = returningRequestRepository;
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
}