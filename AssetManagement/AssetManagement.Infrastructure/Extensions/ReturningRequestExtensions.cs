using AssetManagement.Core.Entities;

namespace AssetManagement.Infrastructure.Extensions;

public static class ReturningRequestExtensions
{
    public static IQueryable<ReturningRequest> Sort(this IQueryable<ReturningRequest> query, string? orderBy)
    {
        query = orderBy switch
        {
            "idAsc" => query.OrderBy(r => r.Id),
            "idDesc" => query.OrderByDescending(r => r.Id),
            
            "assetCodeAsc" => query.OrderBy(r => r.Assignment.AssetCode),
            "assetCodeDesc" => query.OrderByDescending(r => r.Assignment.AssetCode),
            
            "assetNameAsc" => query.OrderBy(r => r.Assignment.Asset.AssetName),
            "assetNameDesc" => query.OrderByDescending(r => r.Assignment.Asset.AssetName),
            
            "requestedByAsc" => query.OrderBy(r => r.RequestedByUser.UserName),
            "requestedByDesc" => query.OrderByDescending(r => r.RequestedByUser.UserName),
            
            "assignedDateAsc" => query.OrderBy(r => r.Assignment.AssignedDate),
            "assignedDateDesc" => query.OrderByDescending(r => r.Assignment.AssignedDate),
            
            "acceptedByAsc" => query.OrderBy(r => r.AcceptedByUser.UserName),
            "acceptedByDesc" => query.OrderByDescending(r => r.AcceptedByUser.UserName),
            
            "returnedDateAsc" => query.OrderBy(r => r.ReturnedDate),
            "returnedDateDesc" => query.OrderByDescending(r => r.ReturnedDate),
            
            "stateAsc" => query.OrderBy(r => r.State),
            "stateDesc" => query.OrderByDescending(r => r.State),
            
            _ => query.OrderBy(r => r.Id)
        };

        return query;
    }

    public static IQueryable<ReturningRequest> Search(this IQueryable<ReturningRequest> query, string? searchTerm)
    {
        if (string.IsNullOrEmpty(searchTerm))
            return query;

        var lowerCaseTerm = searchTerm.ToLower().Trim();
        return query.Where(r => r.Assignment.AssetCode.ToLower().Contains(lowerCaseTerm)
                                || r.Assignment.Asset.AssetName.ToLower().Contains(lowerCaseTerm)
                                || r.RequestedByUser.UserName.ToLower().Contains(lowerCaseTerm));
    }

    public static IQueryable<ReturningRequest> Filter(this IQueryable<ReturningRequest> query, string? stateValues,
        DateOnly? returnedDate)
    {
        if (!string.IsNullOrEmpty(stateValues))
        {
            string[] states = stateValues
                .Split(',')
                .Select(state => state.ToLower().Trim())
                .ToArray();

            query = query.Where(x => states.Contains(
                x.State.ToString().ToLower()));
        }

        if (returnedDate.HasValue)
        {
            query = query.Where(x =>
                x.ReturnedDate.HasValue && DateOnly.FromDateTime(x.ReturnedDate.Value) == returnedDate);
        }

        return query;
    }
}