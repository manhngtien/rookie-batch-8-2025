using AssetManagement.Infrastructure.ConfigurationOptions;
using AssetManagement.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Infrastructure
{
    public static class InfrastructureServiceCollectionExtensions
    {
        public static IServiceCollection AddAssetInfrastructure(this IServiceCollection services, Action<AssetInfrastructureOptions> configureOptions)
        {
            // Add your infrastructure services here
            // For example, database context, repositories, etc.
            var settings = new AssetInfrastructureOptions();
            configureOptions(settings);
            services.Configure(configureOptions);

            // DI DbContext
            //services.AddDbContext<AppDbContext>(options => options.UseSqlServer(settings.ConnectionStrings.Default))
            // DI Scoped Repository
            // DI Services Application
            return services;
        }
    }
}
