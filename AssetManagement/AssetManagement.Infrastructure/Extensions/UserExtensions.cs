using AssetManagement.Core.Entities;
using AssetManagement.Core.Enums;

namespace AssetManagement.Infrastructure.Extensions
{
    public static class UserExtensions
    {
        public static IQueryable<User> Sort(this IQueryable<User> query, string? orderBy)
        {
            query = orderBy switch
            {
                "joinedDateDesc" => query.OrderByDescending(x => x.JoinedDate),
                "fullNameAsc" => query.OrderBy(x => x.FirstName).ThenBy(x => x.LastName),
                "fullNameDesc" => query.OrderByDescending(x => x.FirstName).ThenByDescending(x => x.LastName),
                _ => query.OrderBy(x => x.JoinedDate),
            };

            return query;
        }

        public static IQueryable<User> Search(this IQueryable<User> query, string? searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm)) return query;
            var lowerCaseTerm = searchTerm.Trim().ToLower();
            return query.Where(x => x.UserName.ToLower().Contains(lowerCaseTerm)
                || x.StaffCode.ToString().ToLower().Contains(lowerCaseTerm));
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
