using AssetManagement.Core.Entities;
using AssetManagement.Core.Enums;

namespace AssetManagement.Infrastructure.Extensions
{
    public static class UserExtensions
    {
        public static IQueryable<User> Sort(this IQueryable<User> query, string? orderBy)
        {
            // Default to sort by joined date ascending if no order specified
            if (string.IsNullOrEmpty(orderBy))
            {
                return query.OrderBy(x => x.JoinedDate);
            }

            // Convert to lowercase for case-insensitive comparison
            string lowerOrderBy = orderBy.ToLower();

            // More explicit sorting with "asc" and "desc" suffixes
            query = lowerOrderBy switch
            {
                // Staff Code sorting
                "staffcodeasc" => query.OrderBy(x => x.StaffCode),
                "staffcodedesc" => query.OrderByDescending(x => x.StaffCode),

                // Full Name sorting
                "fullnameasc" => query.OrderBy(x => x.FirstName).ThenBy(x => x.LastName),
                "fullnamedesc" => query.OrderByDescending(x => x.FirstName).ThenByDescending(x => x.LastName),

                // Username sorting
                "usernameasc" => query.OrderBy(x => x.UserName),
                "usernamedesc" => query.OrderByDescending(x => x.UserName),

                // Joined Date sorting
                "joineddateasc" => query.OrderBy(x => x.JoinedDate),
                "joineddatedesc" => query.OrderByDescending(x => x.JoinedDate),

                // Type sorting
                "typeasc" => query.OrderBy(x => x.Type),
                "typedesc" => query.OrderByDescending(x => x.Type),

                // Default to joined date ascending
                _ => query.OrderBy(x => x.JoinedDate)
            };

            return query;
        }



        public static IQueryable<User> Search(this IQueryable<User> query, string? searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm)) return query;
            var lowerCaseTerm = searchTerm.Trim().ToLower();
            return query.Where(x =>
                (x.FirstName.ToLower() + " " + x.LastName.ToLower()).Contains(lowerCaseTerm) ||
                x.StaffCode.ToString().ToLower().Contains(lowerCaseTerm));
        }

        public static IQueryable<User> Filter(this IQueryable<User> query, string? type)
        {
            if (!string.IsNullOrEmpty(type))
            {
                // Try to parse the string into an ERole enum
                if (Enum.TryParse<ERole>(type, out var roleType))
                {
                    // Filter by the parsed enum value
                    query = query.Where(x => x.Type == roleType);
                }
            }

            return query;
        }



    }
}
