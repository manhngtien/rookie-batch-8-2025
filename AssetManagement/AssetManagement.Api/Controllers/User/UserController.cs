// Updated UserController.cs
using AssetManagement.Api.Controllers.Base;
using AssetManagement.Api.Extentions;
using AssetManagement.Application.DTOs.Users;
using AssetManagement.Application.Helpers.Params;
using AssetManagement.Application.Interfaces.User;
using AssetManagement.Application.Interfaces.Auth;
using Microsoft.AspNetCore.Mvc;
using AssetManagement.Core.Exceptions;
using AssetManagement.Infrastructure.Exceptions;

namespace AssetManagement.Api.Controllers.User
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponse>>> GetUsers([FromQuery] UserParams userParams)
        {
            // 1. Get current logged-in user's staff code
            var staffCode = User.GetUserId();

            // 2. Check if user is logged in
            if (string.IsNullOrEmpty(staffCode))
            {
                throw new AppException(ErrorCode.UNAUTHORIZED_ACCESS);
            }

            // 3. Get current user's information
            var currentUser = await _identityService.GetCurrentUserAsync(staffCode);

            // 4. Check if current user is admin
            if (currentUser == null || currentUser.Type != "Admin")
            {
                throw new AppException(ErrorCode.ACCESS_DENIED);
            }

            // 5. Pass to service layer with user params
            var users = await _userService.GetUsersAsync(userParams, currentUser);

            Response.AddPaginationHeader(users.Metadata);
            return Ok(users);
        }

        [HttpGet("{staffCode}")]
        public async Task<ActionResult<UserResponse>> GetUserById(string staffCode)
        {
            // 1. Get current logged-in user's staff code
            var currentUserStaffCode = User.GetUserId();

            // 2. Check if user is logged in
            if (string.IsNullOrEmpty(currentUserStaffCode))
            {
                throw new AppException(ErrorCode.UNAUTHORIZED_ACCESS);
            }

            // 3. Get current user's information
            var currentUser = await _identityService.GetCurrentUserAsync(currentUserStaffCode);

            // 4. Check if current user is admin
            if (currentUser == null || currentUser.Type != "Admin")
            {
                throw new AppException(ErrorCode.ACCESS_DENIED);
            }

            // 5. Pass current user to service to check location-based access
            var user = await _userService.GetUserByIdAsync(staffCode, currentUser);
            return Ok(user);
        }
    }
}
