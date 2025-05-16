using AssetManagement.Core.Interfaces;
using AssetManagement.Infrastructure.ConfigurationOptions;
using AssetManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection;

public static class AssetInfrastructureServiceCollectionExtension
{
    public static IServiceCollection AddAssetInfrastructure(this IServiceCollection services,
        Action<InfrastructureSettings> configureOption)
    {
        var settings = new InfrastructureSettings();
        configureOption(settings);
        services.Configure(configureOption);

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(settings.ConnectionStrings.Default));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        return services;
    }
}