using AssetManagement.Application.DTOs.Accounts;
using AssetManagement.Application.DTOs.Users;
using AssetManagement.Application.Interfaces.Auth;
using AssetManagement.Application.Mappers;
using AssetManagement.Core.Exceptions;
using AssetManagement.Core.Interfaces;
using AssetManagement.Core.Interfaces.Services.Auth;
using AssetManagement.Infrastructure.Exceptions;

namespace AssetManagement.Application.Services.Auth
{
    public class IdentityService : IIdentityService
    {
        private readonly ITokenService _jwt;
        private readonly IUserRepository _userRepository;
        private readonly IAccountRepository _accountRepository;

        public IdentityService(
            ITokenService jwt,
            IUserRepository userRepository,
            IAccountRepository accountRepository)
        {
            _jwt = jwt;
            _userRepository = userRepository;
            _accountRepository = accountRepository;
        }

        public async Task<UserResponse> GetCurrentUserAsync(string staffCode)
        {
            var user = await _userRepository.GetByIdAsync(staffCode);
            if (user == null)
            {
                throw new AppException(ErrorCode.USER_NOT_FOUND);
            }

            return user.MapModelToResponse();
        }

        public async Task<TokenResponse> LoginAsync(LoginRequest loginRequest)
        {
            var account = await _accountRepository.FindByUserNameAsync(loginRequest.UserName);

            if (account == null || !await _accountRepository.CheckPasswordAsync(account, loginRequest.Password))
            {
                throw new AppException(ErrorCode.INVALID_CREDENTIALS);
            }

            var roles = await _accountRepository.GetRolesAsync(account);
            var accessToken = _jwt.GenerateToken(account, roles);
            var refreshToken = _jwt.GenerateRefreshToken(account.Id);
            var refreshTokenExpiresAt = DateTime.UtcNow.AddDays(7);

            var user = await _userRepository.GetByIdAsync(account.StaffCode);
            if (user == null)
            {
                throw new AppException(ErrorCode.USER_NOT_FOUND);
            }

            return new TokenResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                UserResponse = user.MapModelToResponse(),
            };
        }

        public async Task<TokenResponse> RefreshTokenAsync(string? refreshToken)
        {
            if (refreshToken == null)
            {
                throw new AppException(ErrorCode.REFRESH_TOKEN_NOT_FOUND);
            }

            if (!_jwt.ValidateRefreshToken(refreshToken))
            {
                throw new AppException(ErrorCode.INVALID_REFRESH_TOKEN);
            }

            var accountId = _jwt.GetAccountIdFromRefreshToken(refreshToken);
            var account = await _accountRepository.GetByIdAsync(accountId);
            if (account == null)
            {
                throw new AppException(ErrorCode.ACCOUNT_NOT_FOUND);
            }

            var roles = await _accountRepository.GetRolesAsync(account);
            var newAccessToken = _jwt.GenerateToken(account, roles);
            var newRefreshToken = _jwt.GenerateRefreshToken(accountId);
            var newRefreshTokenExpiresAt = DateTime.UtcNow.AddDays(7);

            var user = await _userRepository.GetByIdAsync(account.StaffCode);
            if (user == null)
            {
                throw new AppException(ErrorCode.USER_NOT_FOUND);
            }

            return new TokenResponse
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                UserResponse = user.MapModelToResponse(),
            };
        }

        public async Task ChangePassword(string staffCode, ChangePasswordRequest changePasswordRequest)
        {
            var account = await _accountRepository.GetByStaffCodeAsync(staffCode);

            if (account == null)
            {
                throw new AppException(ErrorCode.ACCOUNT_NOT_FOUND);
            }
            if (!await _accountRepository.CheckPasswordAsync(account, changePasswordRequest.OldPassword))
            {
                throw new AppException(ErrorCode.INVALID_CREDENTIALS);
            }

            var result = await _accountRepository.ChangePasswordAsync(account, changePasswordRequest.NewPassword);

            if (!result)
            {
                throw new AppException(ErrorCode.SAVE_ERROR);
            }
        }
    }
}