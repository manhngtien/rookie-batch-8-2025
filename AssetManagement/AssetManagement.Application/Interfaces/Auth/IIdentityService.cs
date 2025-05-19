using AssetManagement.Application.DTOs.Accounts;
using AssetManagement.Application.DTOs.Users;

namespace AssetManagement.Application.Interfaces.Auth
{
    public interface IIdentityService
    {
        Task ChangePassword(string staffCode, ChangePasswordRequest changePasswordRequest);
        Task<UserResponse> GetCurrentUserAsync(string staffCode);
        Task<TokenResponse> LoginAsync(LoginRequest loginRequest);
        Task<TokenResponse> RefreshTokenAsync(string? refreshToken);
    }
}
