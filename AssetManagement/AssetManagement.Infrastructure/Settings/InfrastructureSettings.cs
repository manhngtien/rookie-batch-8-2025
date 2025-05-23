using AssetManagement.Api.Settings;
using AssetManagement.Infrastructure.Settings.Options;

namespace AssetManagement.Infrastructure.Settings;

public class InfrastructureSettings
{
    public ConnectionStringsOption ConnectionStringsOption { get; set; } = new ConnectionStringsOption();
    public JwtOption JwtOption { get; set; } = new JwtOption();
}