using AssetManagement.Application.DTOs.ReturningRequests;
using AssetManagement.Core.Entities;
using AssetManagement.Core.Enums;

namespace AssetManagement.Application.Mappers;

public static class ReturningRequestMapper
{
    public static ReturningRequestResponse MapModelToResponse(this ReturningRequest returningRequest)
    {
        return new ReturningRequestResponse
        {
            Id = returningRequest.Id,
            State = Enum.GetName(typeof(ReturningRequestStatus), returningRequest.State)!,
            ReturnedDate = (returningRequest.ReturnedDate.HasValue)
                ? DateOnly.FromDateTime(returningRequest.ReturnedDate.Value)
                : null,
            RequestedBy = returningRequest.RequestedByUser.UserName,
            AcceptedBy = (returningRequest.RequestedByUser != null) ? returningRequest.RequestedByUser.UserName : "",
            AssetCode = (returningRequest.Assignment != null) ? returningRequest.Assignment.AssetCode : "",
            AssetName = (returningRequest.Assignment != null && returningRequest.Assignment.Asset != null)
                ? returningRequest.Assignment.Asset.AssetName
                : "",
            AssignedDate = DateOnly.FromDateTime(returningRequest.Assignment!.AssignedDate)
        };
    }
}