using AssetManagement.Core.Entities;

namespace AssetManagement.Application.Extensions
{
    public static class AssignmentExtensions
    {
        public static IQueryable<Assignment> Sort(this IQueryable<Assignment> query, string? orderBy)
        {
            // Default to sort by AssignedDate ascending if no order specified
            if (string.IsNullOrEmpty(orderBy))
            {
                return query.OrderBy(a => a.AssignedDate);
            }

            // Convert to lowercase for case-insensitive comparison
            string lowerOrderBy = orderBy.ToLower();

            // More explicit sorting with "asc" and "desc" suffixes
            query = lowerOrderBy switch
            {
                // Id sorting
                "idasc" => query.OrderBy(a => a.Id),
                "iddesc" => query.OrderByDescending(a => a.Id),

                // State sorting
                "stateasc" => query.OrderBy(a => a.State),
                "statedesc" => query.OrderByDescending(a => a.State),

                // AssetCode sorting
                "assetcodeasc" => query.OrderBy(a => a.AssetCode),
                "assetcodedesc" => query.OrderByDescending(a => a.AssetCode),

                // AssetName sorting
                "assetnameasc" => query.OrderBy(a => a.Asset.AssetName),
                "assetnamedesc" => query.OrderByDescending(a => a.Asset.AssetName),

                // AssignedTo sorting
                "assignedtoasc" => query.OrderBy(a => a.AssignedToUser.UserName),
                "assignedtodesc" => query.OrderByDescending(a => a.AssignedToUser.UserName),

                // AssignedBy sorting
                "assignedbyasc" => query.OrderBy(a => a.AssignedByUser.UserName),
                "assignedbydesc" => query.OrderByDescending(a => a.AssignedByUser.UserName),

                // AssignedDate sorting
                "assigneddateasc" => query.OrderBy(a => a.AssignedDate),
                "assigneddatedesc" => query.OrderByDescending(a => a.AssignedDate),

                // Default to AssignedDate ascending
                _ => query.OrderBy(a => a.AssignedDate)
            };

            return query;
        }

        public static IQueryable<Assignment> Search(this IQueryable<Assignment> query, string? searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm)) return query;

            var lowerCaseTerm = searchTerm.Trim().ToLower();
            return query.Where(a =>
                (a.AssetCode != null && a.AssetCode.ToLower().Contains(lowerCaseTerm)) ||
                (a.Asset != null && a.Asset.AssetName != null && a.Asset.AssetName.ToLower().Contains(lowerCaseTerm)) ||
                (a.AssignedToUser != null && a.AssignedToUser.UserName != null && a.AssignedToUser.UserName.ToLower().Contains(lowerCaseTerm)));
        }

        public static IQueryable<Assignment> Filter(this IQueryable<Assignment> query, string? stateValues, DateOnly? assignedDate)
        {
            if (!string.IsNullOrEmpty(stateValues))
            {
                string[] states = stateValues
                    .Split(',')
                    .Select(state => state.ToLower().Trim())
                    .ToArray();

                query = query.Where(x => states.Contains(x.State.ToString().ToLower()));
            }

            if (assignedDate.HasValue)
            {
                query = query.Where(a => DateOnly.FromDateTime(a.AssignedDate) == assignedDate.Value);
            }

            return query;
        }
    }
}