using AssetManagement.Core.Entities;

namespace AssetManagement.Infrastructure.Extensions
{
    public static class AssetExtensions
    {
        public static IQueryable<Asset> Sort(this IQueryable<Asset> query, string? orderBy)
        {
            query = orderBy?.ToLower() switch
            {
                "assetcodedesc" => query.OrderByDescending(x => x.AssetCode),
                "assetnameasc" => query.OrderBy(x => x.AssetName),
                "assetnamedesc" => query.OrderByDescending(x => x.AssetName),
                "categoryasc" => query.OrderBy(x => x.Category.CategoryName),
                "categorydesc" => query.OrderByDescending(x => x.Category.CategoryName),
                "stateasc" => query.OrderBy(x => x.State.ToString()),
                "statedesc" => query.OrderByDescending(x => x.State.ToString()),
                _ => query.OrderBy(x => x.AssetCode),
            };

            return query;
        }

        public static IQueryable<Asset> Search(this IQueryable<Asset> query, string? searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return query;
            }

            var lowerCaseTerm = searchTerm.Trim().ToLower();

            return query.Where(
                x => x.AssetName.Contains(lowerCaseTerm, StringComparison.OrdinalIgnoreCase)
                || x.AssetCode.Contains(lowerCaseTerm, StringComparison.OrdinalIgnoreCase)
            );
        }

        public static IQueryable<Asset> Filter(this IQueryable<Asset> query, string? categoryName, string? state)
        {
            if (!string.IsNullOrEmpty(categoryName))
            {
                query = query.Where(x => x.Category.CategoryName.Equals(categoryName, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(state))
            {
                query = query.Where(x => x.State.ToString().Equals(state, StringComparison.OrdinalIgnoreCase));
            }

            return query;
        }
    }
}
