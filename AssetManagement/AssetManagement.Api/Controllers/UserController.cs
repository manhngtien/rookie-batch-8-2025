// Updated UserController.cs
using AssetManagement.Api.Controllers.Base;
using AssetManagement.Api.Extensions;
using AssetManagement.Application.DTOs.Users;
using AssetManagement.Application.Helpers.Params;
using AssetManagement.Application.Interfaces;
using AssetManagement.Application.Interfaces.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssetManagement.Api.Controllers
{
    public class UserController : BaseApiController
    {
        private readonly IUserService _userService;
        private readonly IIdentityService _identityService;

        public UserController(IUserService userService, IIdentityService identityService)
        {
            _userService = userService;
            _identityService = identityService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponse>>> GetUsers([FromQuery] UserParams userParams)
        {
            // Get current logged-in user's staff code
            var staffCode = User.GetUserId();

            // Get the admin's location
            var adminLocation = await _userService.GetLocationByStaffCodeAsync(staffCode);

            // Get users filtered by the admin's location
            var users = await _userService.GetUsersAsync(userParams, adminLocation);

            Response.AddPaginationHeader(users.Metadata);
            return Ok(users);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{staffCode}")]
        public async Task<ActionResult<UserResponse>> GetUserById(string staffCode)
        {
            // Get current logged-in user's staff code
            var currentUserStaffCode = User.GetUserId();

            // Get the admin's location
            var adminLocation = await _userService.GetLocationByStaffCodeAsync(currentUserStaffCode);
            // Get user and verify location-based access
            var user = await _userService.GetUserByIdAsync(staffCode, adminLocation);
            return Ok(user);
        }
    }
}
