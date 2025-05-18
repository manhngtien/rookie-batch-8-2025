using AssetManagement.Api.Controllers.Base;
using AssetManagement.Api.Extentions;
using AssetManagement.Application.DTOs.Accounts;
using AssetManagement.Application.Interfaces.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssetManagement.Api.Controllers.Auth
{
    public class AuthController : BaseApiController
    {
        private readonly IIdentityService _identityService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthController(IIdentityService identityService, IHttpContextAccessor httpContextAccessor)
        {
            _identityService = identityService;
            _httpContextAccessor = httpContextAccessor;
        }

        private void SetAuthCookies(string accessToken, string refreshToken)
        {
            var jwtCookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddHours(1),
                Path = "/"
            };
            _httpContextAccessor.HttpContext!.Response.Cookies.Append("auth_jwt", accessToken, jwtCookieOptions);

            var refreshCookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(7),
                Path = "/"
            };
            _httpContextAccessor.HttpContext.Response.Cookies.Append("refresh", refreshToken, refreshCookieOptions);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var tokenResponse = await _identityService.LoginAsync(loginRequest);
            SetAuthCookies(tokenResponse.AccessToken, tokenResponse.RefreshToken);
            return Ok(tokenResponse.UserResponse);
        }

        [HttpGet("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refresh"];

            var tokenResponse = await _identityService.RefreshTokenAsync(refreshToken);
            SetAuthCookies(tokenResponse.AccessToken, tokenResponse.RefreshToken);
            return Ok(tokenResponse.UserResponse);
        }

        [HttpGet("check")]
        public async Task<IActionResult> CheckAuth()
        {
            var staffCode = User.GetUserId();
            var userResponse = await _identityService.GetCurrentUserAsync(staffCode);

            return Ok(userResponse);
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            _httpContextAccessor.HttpContext!.Response.Cookies.Delete("auth_jwt");
            _httpContextAccessor.HttpContext.Response.Cookies.Delete("refresh");
            return NoContent();
        }
    }
}

