using AssetManagement.Application.Helpers.Params;
using AssetManagement.Application.Services;
using AssetManagement.Core.Entities;
using AssetManagement.Core.Interfaces;
using AssetManagement.UnitTests.Helpers;
using Moq;

namespace AssetManagement.UnitTests.Services;

public class ReportServiceTests
{
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
    private readonly Mock<IAssetRepository> _assetRepositoryMock;
    private readonly ReportService _reportService;
    
    public ReportServiceTests()
    {
        _categoryRepositoryMock = new Mock<ICategoryRepository>();
        _assetRepositoryMock = new Mock<IAssetRepository>();
        _reportService = new ReportService(_categoryRepositoryMock.Object, _assetRepositoryMock.Object);
    }

    [Fact]
    public async Task GetReportsAsync_ShouldReturnCorrectlyAggregatedReports()
    {
        // Arrange
        var categories = Faker.GetCategories();
        var assets = Faker.GetAssets();

        _categoryRepositoryMock.Setup(r => r.GetAllAsync()).Returns(categories);
        _assetRepositoryMock.Setup(r => r.GetAllAsync()).Returns(assets);
        
        var reportParams = new ReportParams { OrderBy = "categorynameasc" };

        // Act
        var result = await _reportService.GetReportsAsync(reportParams);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetReportsAsync_WithNoCategories_ShouldReturnEmptyList()
    {
        // Arrange
        var categories = new List<Category>().AsQueryable();
        var assets = new List<Asset>().AsQueryable();
        
        _categoryRepositoryMock.Setup(r => r.GetAllAsync()).Returns(categories);
        _assetRepositoryMock.Setup(r => r.GetAllAsync()).Returns(assets);
        var reportParams = new ReportParams { OrderBy = "categorynameasc" };

        // Act
        var result = await _reportService.GetReportsAsync(reportParams);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetReportsAsync_WithNoAssets_ShouldReturnCategoriesWithZeroCounts()
    {
        // Arrange
        var categories = new List<Category>
        {
            new Category { Id = 1, CategoryName = "Laptops", Prefix = "LA", Total = 0 }
        }.AsQueryable();
        
        var assets = new List<Asset>().AsQueryable();
        
        _categoryRepositoryMock.Setup(r => r.GetAllAsync()).Returns(categories);
        _assetRepositoryMock.Setup(r => r.GetAllAsync()).Returns(assets);
        var reportParams = new ReportParams { OrderBy = "categorynameasc" };

        // Act
        var result = await _reportService.GetReportsAsync(reportParams);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);

        var report = result.First();
        Assert.Equal("Laptops", report.CategoryName);
        Assert.Equal(0, report.Total);
        Assert.Equal(0, report.TotalAssigned);
        Assert.Equal(0, report.TotalAvailable);
        Assert.Equal(0, report.TotalNotAvailable);
        Assert.Equal(0, report.TotalWaitingForRecycling);
        Assert.Equal(0, report.TotalRecycled);
    }
    
    [Fact]
    public async Task GetReportsAsync_WithCategoryNameDesc_ShouldSortDescending()
    {
        // Arrange
        var categories = Faker.GetCategories();
        var assets = Faker.GetAssets();

        _categoryRepositoryMock.Setup(r => r.GetAllAsync()).Returns(categories);
        _assetRepositoryMock.Setup(r => r.GetAllAsync()).Returns(assets);
        var reportParams = new ReportParams { OrderBy = "categorynamedesc" };

        // Act
        var result = await _reportService.GetReportsAsync(reportParams);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.Equal("Monitors", result.ElementAt(0).CategoryName);
        Assert.Equal("Laptops", result.ElementAt(1).CategoryName);
    }
}