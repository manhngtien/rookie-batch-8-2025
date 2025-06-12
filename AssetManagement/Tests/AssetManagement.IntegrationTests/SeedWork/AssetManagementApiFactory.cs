using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using AssetManagement.Core.Entities;
using AssetManagement.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using AssetManagement.Api;
using Microsoft.Extensions.Configuration;

namespace AssetManagement.IntegrationTests.SeedWork;

public class AssetManagementApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    // private DbContextOptions<AppDbContext> _dbContextOptions = null!;
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");
        builder.ConfigureAppConfiguration((context, config) =>
        {
            config.AddEnvironmentVariables();
        });
        
        builder.ConfigureTestServices(services =>
        {
            var dbContextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
            if (dbContextDescriptor != null)
            {
                services.Remove(dbContextDescriptor);
            }

            var appDbContextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(AppDbContext));
            if (appDbContextDescriptor != null)
            {
                services.Remove(appDbContextDescriptor);
            }
            
            var configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
            var connectionString = configuration.GetValue<string>("InfrastructureSettings:ConnectionStringsOption:Default");

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });
            
            services.AddIdentityCore<Account>() 
                .AddRoles<IdentityRole<Guid>>()
                .AddEntityFrameworkStores<AppDbContext>();
        });
    }
    
    public HttpClient CreateAuthenticatedClient(string role, string staffCode, string userName)
    {
        var client = CreateClient();
        var token = GenerateTestJwt();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return client;
    }
    private string GenerateTestJwt()
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "SD0003"), // StaffCode of the test admin
            new Claim(ClaimTypes.Name, "baoh"),
            new Claim(ClaimTypes.Role, "Admin")
        };

        var key = new SymmetricSecurityKey("TANGTIENLUAN@!)@21022003NASHTECHROOKIEBATCH8"u8.ToArray());
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "AssetManagement",
            audience: "AssetManagementOffice",
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(15),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task InitializeAsync()
    {
        using var scope = Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await context.Database.EnsureCreatedAsync();
    }

    public async Task DisposeAsync()
    {
        await base.DisposeAsync();
    }
}