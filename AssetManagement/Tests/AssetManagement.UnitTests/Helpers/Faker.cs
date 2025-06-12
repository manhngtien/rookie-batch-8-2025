using AssetManagement.Core.Entities;
using AssetManagement.Core.Enums;

namespace AssetManagement.UnitTests.Helpers;

public static class Faker
{
    public static IQueryable<Category> GetCategories()
    {
        return new List<Category>
        {
            new Category { Id = 1, CategoryName = "Laptops", Prefix = "LA", Total = 5 },
            new Category { Id = 2, CategoryName = "Monitors", Prefix = "MO", Total = 3 }
        }.AsQueryable();
    }

    public static IQueryable<Asset> GetAssets()
    {
        return new List<Asset>
        {
            new Asset { CategoryId = 1, State = AssetStatus.Assigned, AssetCode = "LA001", AssetName = "Laptop 1", Specification = "Spec", Location = ELocation.HCM, InstalledDate = DateTime.UtcNow },
            new Asset { CategoryId = 1, State = AssetStatus.Assigned, AssetCode = "LA002", AssetName = "Laptop 2", Specification = "Spec", Location = ELocation.HCM, InstalledDate = DateTime.UtcNow },
            new Asset { CategoryId = 1, State = AssetStatus.Available, AssetCode = "LA003", AssetName = "Laptop 3", Specification = "Spec", Location = ELocation.HCM, InstalledDate = DateTime.UtcNow },
            new Asset { CategoryId = 1, State = AssetStatus.Waiting_For_Recycling, AssetCode = "LA004", AssetName = "Laptop 4", Specification = "Spec", Location = ELocation.HCM, InstalledDate = DateTime.UtcNow },
            new Asset { CategoryId = 1, State = AssetStatus.Recycled, AssetCode = "LA005", AssetName = "Laptop 5", Specification = "Spec", Location = ELocation.HCM, InstalledDate = DateTime.UtcNow },
            new Asset { CategoryId = 2, State = AssetStatus.Not_Available, AssetCode = "MO001", AssetName = "Monitor 1", Specification = "Spec", Location = ELocation.HCM, InstalledDate = DateTime.UtcNow },
            new Asset { CategoryId = 2, State = AssetStatus.Available, AssetCode = "MO002", AssetName = "Monitor 2", Specification = "Spec", Location = ELocation.HCM, InstalledDate = DateTime.UtcNow },
            new Asset { CategoryId = 2, State = AssetStatus.Available, AssetCode = "MO003", AssetName = "Monitor 3", Specification = "Spec", Location = ELocation.HCM, InstalledDate = DateTime.UtcNow }
        }.AsQueryable();
    }
}