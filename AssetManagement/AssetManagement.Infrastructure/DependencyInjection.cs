using AssetManagement.Core.Entities;
using AssetManagement.Core.Interfaces;
using AssetManagement.Infrastructure.Settings.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddAssetInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        // Read Configuration Options From AppSettings
        var connectionStringsOption = new ConnectionStringsOption();
        configuration.GetSection("InfrastructureSettings:ConnectionStringsOption").Bind(connectionStringsOption);

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionStringsOption.Default));

        services.AddIdentity<Account, IdentityRole<Guid>>(opt =>
        {
            opt.Password.RequireDigit = true;
            opt.Password.RequireLowercase = true;
            opt.Password.RequireNonAlphanumeric = true;
            opt.Password.RequireUppercase = false;
            opt.Password.RequiredLength = 6;
            opt.Password.RequiredUniqueChars = 1;
        })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        // Dependencies Services, Repos
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<IAssetRepository, AssetRepository>();
        services.AddScoped<IAssignmentRepository, AssignmentRepository>();
        services.AddScoped<IReturningRequestRepository, ReturningRequestRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        return services;
    }
}
