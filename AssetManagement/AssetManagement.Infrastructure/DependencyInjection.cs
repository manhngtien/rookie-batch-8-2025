using AssetManagement.Application.Interfaces;
using AssetManagement.Application.Interfaces.Auth;
using AssetManagement.Application.Services;
using AssetManagement.Application.Services.Auth;
using AssetManagement.Application.Validators.Accounts;
using AssetManagement.Core.Entities;
using AssetManagement.Core.Interfaces;
using AssetManagement.Core.Interfaces.Services.Auth;
using AssetManagement.Infrastructure.Services;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddAssetInfrastructure(this IServiceCollection services,
        Action<InfrastructureSettings> configureOptions)
    {
        // Read Configuration Options From AppSettings
        var settings = new InfrastructureSettings();
        configureOptions(settings);
        services.Configure(configureOptions);
        
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(settings.ConnectionStringsOption.Default));

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

        // Dependencies Repos
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<IAssetRepository, AssetRepository>();
        services.AddScoped<IAssignmentRepository, AssignmentRepository>();
        services.AddScoped<IReturningRequestRepository, ReturningRequestRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();

        // Dependencies Services
        services.AddValidatorsFromAssembly(typeof(ChangePasswordRequestValidator).Assembly);

        services.AddScoped<ITokenService, JwtService>(_ =>
        {
            return new JwtService(settings.JwtOption);
        });
        
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAssignmentService, AssignmentService>();
        services.AddScoped<IReturningRequestService, ReturningRequestService>();
        services.AddScoped<IAssetService, AssetService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IReportService, ReportService>();
        
        return services;
    }
}
