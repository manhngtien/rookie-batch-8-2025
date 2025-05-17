using AssetManagement.Application.DTOs.Users;

namespace AssetManagement.Application.DTOs.Accounts
{
    public class TokenResponse
    {
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
        public required UserResponse UserResponse { get; set; }
    }
}
