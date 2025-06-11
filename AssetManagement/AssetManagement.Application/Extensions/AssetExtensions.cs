using AssetManagement.Core.Entities;

namespace AssetManagement.Application.Extensions
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
                x => x.AssetName.ToLower().Contains(lowerCaseTerm)
                || x.AssetCode.ToLower().Contains(lowerCaseTerm)
            );
        }

        public static IQueryable<Asset> Filter(this IQueryable<Asset> query, string? categoryNameValues, string? stateValues)
        {
            if (!string.IsNullOrEmpty(categoryNameValues))
            {
                string[] categoryNames = categoryNameValues
                    .Split(',')
                    .Select(categoryName => categoryName.ToLower().Trim())
                    .ToArray();

                query = query.Where(x => categoryNames.Contains(x.Category.CategoryName.ToLower()));
            }

            if (!string.IsNullOrEmpty(stateValues))
            {
                string[] states = stateValues
                    .Split(',')
                    .Select(state => state.ToLower().Trim())
                    .ToArray();

                query = query.Where(x => states.Contains(x.State.ToString().ToLower()));
            }

            return query;
        }
    }
}
