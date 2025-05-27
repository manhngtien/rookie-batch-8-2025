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
    public class UsersController : BaseApiController
    {
        private readonly IUserService _userService;
        private readonly IIdentityService _identityService;

        public UsersController(IUserService userService, IIdentityService identityService)
        {
            _userService = userService;
            _identityService = identityService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<UserResponse>>> GetUsers([FromQuery] UserParams userParams)
        {
            // Get current logged-in user's staff code
            var staffCode = User.GetUserId();

            // Get the admin's location
            var adminLocation = await _userService.GetLocationByStaffCodeAsync(staffCode);

            // Get users filtered by the admin's location, excluding the current user
            var users = await _userService.GetUsersAsync(userParams, adminLocation, staffCode);

            Response.AddPaginationHeader(users.Metadata);
            return Ok(users);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{staffCode}")]
        public async Task<ActionResult<UserResponse>> GetUserById(string staffCode)
        {
            // Get current logged-in user's staff code
            var currentUserStaffCode = User.GetUserId();

            // Prevent accessing the current user's details
            if (staffCode == currentUserStaffCode)
            {
                return BadRequest("Cannot access your own user details in this context.");
            }

            // Get the admin's location
            var adminLocation = await _userService.GetLocationByStaffCodeAsync(currentUserStaffCode);
            // Get user and verify location-based access
            var user = await _userService.GetUserByIdAsync(staffCode, adminLocation);
            return Ok(user);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateUser([FromForm] CreateUserRequest createUserRequest)
        {
            // Get current logged-in user's staff code
            var staffCode = User.GetUserId();

            var createdUser = await _userService.CreateUserAsync(createUserRequest, staffCode);

            // Trả về CreatedAtAction với staffCode và đường dẫn đến action GetUserById
            return CreatedAtAction(nameof(GetUserById),
                                new { staffCode = createdUser.StaffCode },
                                createdUser);
        }



    }
}
