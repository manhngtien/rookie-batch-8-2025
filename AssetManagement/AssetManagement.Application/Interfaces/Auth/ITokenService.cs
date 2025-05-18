using AssetManagement.Core.Entities;

namespace AssetManagement.Core.Interfaces.Services.Auth
{
    public interface ITokenService
    {
        string GenerateRefreshToken(Guid accountId);
        string GenerateToken(Account user, IList<string> roles);
        Guid GetAccountIdFromRefreshToken(string refreshToken);
        bool ValidateRefreshToken(string refreshToken);
    }
}
