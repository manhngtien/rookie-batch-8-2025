using AssetManagement.Core.Entities;
using AssetManagement.Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection;

public static class AssetInfrastructureServiceCollectionExtension
{
    public static IServiceCollection AddAssetInfrastructure(this IServiceCollection services,
        Action<InfrastructureSettings> configureOption)
    {
        // Read Configuration Options From AppSettings
        var settings = new InfrastructureSettings();
        configureOption(settings);
        services.Configure(configureOption);
        
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(settings.ConnectionStrings.Default));
        
        services.AddIdentity<Account, IdentityRole<Guid>>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();
        
        // Dependencies Services, Repos
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IAssetRepository, AssetRepository>();
        return services;
    }
}