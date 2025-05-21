using AssetManagement.Core.Entities;
using AssetManagement.Core.Enums;
using AssetManagement.Core.Interfaces;

namespace AssetManagement.Infrastructure.Repositories
{
    public class AssetRepository : IAssetRepository
    {
        private readonly List<Asset> _mockAssets;

        public AssetRepository()
        {
            var mockCategory = new Category
            {
                Id = 1,
                Prefix = "ELEC",
                CategoryName = "Electronics",
                Total = 50,
                Assets = []
            };
            
            var mockAssets = new List<Asset>
            {
                new()
                {
                    AssetCode = "A001",
                    AssetName = "Laptop Dell XPS 15",
                    Specification = "Intel Core i7, 16GB RAM, 512GB SSD",
                    State = AssetStatus.Available,
                    Location = ELocation.HCM,
                    InstalledDate = new DateTime(2023, 5, 10),
                    CategoryId = 1,
                    Category = mockCategory
                },
                new()
                {
                    AssetCode = "A002",
                    AssetName = "HP Laser Printer",
                    Specification = "Color LaserJet Pro, Wi-Fi enabled",
                    State = AssetStatus.Available,
                    Location = ELocation.HN,
                    InstalledDate = new DateTime(2022, 8, 15),
                    CategoryId = 1,
                    Category = mockCategory
                },
                new()
                {
                    AssetCode = "A003",
                    AssetName = "Ergonomic Office Chair",
                    Specification = "Mesh back, adjustable armrests",
                    State = AssetStatus.Available,
                    Location = ELocation.DN,
                    InstalledDate = new DateTime(2020, 3, 5),
                    CategoryId = 1,
                    Category = mockCategory
                },
                new()
                {
                    AssetCode = "A004",
                    AssetName = "Samsung 24-inch Monitor",
                    Specification = "Full HD, 75Hz, HDMI & DisplayPort",
                    State = AssetStatus.Available,
                    Location = ELocation.HCM,
                    InstalledDate = new DateTime(2021, 11, 20),
                    CategoryId = 1,
                    Category = mockCategory
                },
                new()
                {
                    AssetCode = "A005",
                    AssetName = "Mechanical Keyboard - Keychron K8",
                    Specification = "Bluetooth/Wired, RGB Backlight, Brown Switches",
                    State = AssetStatus.Available,
                    Location = ELocation.HN,
                    InstalledDate = new DateTime(2023, 1, 15),
                    CategoryId = 1,
                    Category = mockCategory
                },
                new()
                {
                    AssetCode = "A006",
                    AssetName = "Sony Noise-Canceling Headphones",
                    Specification = "Over-ear, Bluetooth, ANC, 30-hour battery life",
                    State = AssetStatus.Available,
                    Location = ELocation.DN,
                    InstalledDate = new DateTime(2022, 6, 5),
                    CategoryId = 1,
                    Category = mockCategory
                }
            };
            
            _mockAssets = mockAssets;
        }
        
        public IQueryable<Asset> GetAllAsync()
        {
            return _mockAssets.AsQueryable();
        }

        public Task<Asset?> GetByIdAsync<TId>(TId id)
        {
            throw new NotImplementedException();
        }

        public async Task<Asset?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<Asset?> CreateAsync(Asset asset)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(Asset asset)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(Asset asset)
        {
            throw new NotImplementedException();    
        }
    }
}
