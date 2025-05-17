using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Core.Interfaces.Services.Auth
{
    public interface IIdentityService
    {
        Task<UserResponse> GetCurrentUserAsync(Guid userId);
        Task<TokenResponse> LoginAsync(LoginRequest loginRequest);
        Task<TokenResponse> RefreshTokenAsync(RefreshTokenRequest dto);
        Task<TokenResponse> RegisterAsync(RegisterRequest registerRequest);
    }
}
