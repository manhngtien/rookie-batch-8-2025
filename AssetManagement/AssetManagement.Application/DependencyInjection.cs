using AssetManagement.Api.Settings;
using AssetManagement.Application.Interfaces;
using AssetManagement.Application.Interfaces.Auth;
using AssetManagement.Application.Services;
using AssetManagement.Application.Services.Auth;
using AssetManagement.Core.Interfaces.Services.Auth;
using AssetManagement.Infrastructure.Settings.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAssetApplication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtOption = new JwtOption();
            configuration.GetSection("InfrastructureSettings:JwtOption").Bind(jwtOption);

            services.AddScoped<ITokenService, JwtService>(_ =>
            {
                return new JwtService(jwtOption);
            });
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAssignmentService, AssignmentService>();
            services.AddScoped<IReturningRequestService, ReturningRequestService>();
            services.AddScoped<IAssetService, AssetService>();
            return services;
        }
    }
}
