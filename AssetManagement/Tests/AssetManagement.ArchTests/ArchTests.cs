using System.Reflection;
using AssetManagement.Api.Controllers.Base;
using NetArchTest.Rules;

namespace AssetManagement.ArchTests;

public class ArchTests
{
    private const string ApiNamespace = "AssetManagement.Api";
    private const string ApplicationNamespace = "AssetManagement.Application";
    private const string CoreNamespace = "AssetManagement.Core";
    private const string InfrastructureNamespace = "AssetManagement.Infrastructure";

    [Fact]
    public void Core_Should_Not_HaveDependenciesOnOtherProjects()
    {
        // Arrange
        var assembly = Assembly.Load(CoreNamespace);

        var otherProjects = new[]
        {
            ApiNamespace,
            ApplicationNamespace,
            InfrastructureNamespace
        };
        
        // Act
        var result = Types.InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAll(otherProjects)
            .GetResult();
        
        // Assert
        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void Application_Should_Not_HaveDependenciesOnApi()
    {
        // Arrange
        var assembly = Assembly.Load(ApplicationNamespace);
        
        // Act
        var result = Types
            .InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAny(ApiNamespace)
            .GetResult();
        
        // Assert
        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void Application_Should_Not_HaveDependenciesOnInfrastructure()
    {
        // Arrange
        var assembly = Assembly.Load(ApplicationNamespace);
        
        // Act
        var result = Types
            .InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAny(InfrastructureNamespace)
            .GetResult();
        
        // Assert
        Assert.True(result.IsSuccessful);
    }
    
    [Fact]
    public void Controllers_Should_EndWithController()
    {
        // Arrange
        var assembly = Assembly.Load(ApiNamespace);
        
        // Act
        var result = Types
            .InAssembly(assembly)
            .That()
            .Inherit(typeof(BaseApiController))
            .Should()
            .HaveNameEndingWith("Controller")
            .GetResult();
        
        // Assert
        Assert.True(result.IsSuccessful);
    }
    
    [Fact]
    public void Repositories_Should_EndWithRepository()
    {
        // Arrange
        var assembly = Assembly.Load(InfrastructureNamespace);
        
        // Act
        var result = Types
            .InAssembly(assembly)
            .That()
            .ResideInNamespaceEndingWith("Repositories")
            .Should()
            .HaveNameEndingWith("Repository")
            .GetResult();
        
        // Assert
        Assert.True(result.IsSuccessful);
    }
}