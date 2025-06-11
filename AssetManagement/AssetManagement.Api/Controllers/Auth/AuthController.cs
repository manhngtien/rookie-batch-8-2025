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
                SameSite = SameSiteMode.None,
                Expires = DateTime.Now.AddHours(1),
                Path = "/"
            };
            _httpContextAccessor.HttpContext!.Response.Cookies.Append("auth_jwt", accessToken, jwtCookieOptions);

            var refreshCookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.Now.AddDays(7),
                Path = "/"
            };
            _httpContextAccessor.HttpContext.Response.Cookies.Append("refresh", refreshToken, refreshCookieOptions);
        }

        private void ClearAuthCookies()
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Path = "/"
            };
            _httpContextAccessor.HttpContext!.Response.Cookies.Delete("auth_jwt", cookieOptions);
            _httpContextAccessor.HttpContext.Response.Cookies.Delete("refresh", cookieOptions);
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


        [HttpPost("logout")]
        public IActionResult Logout()
        {
            ClearAuthCookies();
            return NoContent();
        }

        [HttpGet("check")]
        [Authorize]
        public async Task<IActionResult> CheckAuth()
        {
            var staffCode = User.GetUserId();
            var userResponse = await _identityService.GetCurrentUserAsync(staffCode);

            return Ok(userResponse);
        }

        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest changePasswordRequest)
        {
            var staffCode = User.GetUserId();
            await _identityService.ChangePassword(staffCode, changePasswordRequest);
            return NoContent();
        }
    }
}

