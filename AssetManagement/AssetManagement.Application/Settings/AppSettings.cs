using AssetManagement.Infrastructure.ConfigurationOptions;

namespace AssetManagement.Api.Settings;

public class AppSettings
{
    public JwtSettings JwtSettings { get; set; }
    public InfrastructureSettings InfrastructureSettings { get; set; }
}