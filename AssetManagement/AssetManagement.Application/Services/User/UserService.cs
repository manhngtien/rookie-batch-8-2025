// Updated UserService.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AssetManagement.Application.DTOs.Users;
using AssetManagement.Application.Helpers.Params;
using AssetManagement.Application.Interfaces.User;
using AssetManagement.Application.Mappers;
using AssetManagement.Application.Paginations;
using AssetManagement.Core.Entities;
using AssetManagement.Core.Enums;
using AssetManagement.Core.Exceptions;
using AssetManagement.Core.Interfaces;
using AssetManagement.Infrastructure.Exceptions;
using AssetManagement.Infrastructure.Extensions;

namespace AssetManagement.Application.Services.User
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<PagedList<UserResponse>> GetUsersAsync(UserParams userParams, UserResponse currentUser)
        {
            // First, get all users
            var query = _userRepository.GetAllAsync();

            // Ensure the user is admin and filter users based on the admin's location
            if (Enum.TryParse<ERole>(currentUser.Type, out var userRole) && userRole == ERole.Admin)
            {
                
                // Filter users who have same location as the logged-in admin
                if (Enum.TryParse<ELocation>(currentUser.Location, out var userLocation))
                {
                    query = query.Where(u => u.Location == userLocation);
                }
            }
            else
            {
                // If not an admin, return empty result
                return await PaginationService.ToPagedList(
                    Enumerable.Empty<UserResponse>().AsQueryable(),
                    userParams.PageNumber,
                    userParams.PageSize
                );
            }

            // Apply additional filters, sorting, and searching
            query = query
                .Sort(userParams.OrderBy)
                .Search(userParams.SearchTerm)
                .Filter(userParams.Type);

            // Map to DTOs
            var projectedQuery = query.Select(x => x.MapModelToResponse());

            return await PaginationService.ToPagedList(
                projectedQuery,
                userParams.PageNumber,
                userParams.PageSize
            );
        }

        public async Task<UserResponse> GetUserByIdAsync(string staffCode, UserResponse currentUser)
        {
            var user = await _userRepository.GetByIdAsync(staffCode);
            if (user == null)
            {
                var attributes = new Dictionary<string, object>
                {
                    { "staffCode", staffCode }
                };
                throw new AppException(ErrorCode.USER_NOT_FOUND, attributes);
            }

            // Check if current user (admin) has the same location as the requested user
            if (Enum.TryParse<ERole>(currentUser.Type, out var userRole) && userRole == ERole.Admin)
            {
                if (Enum.TryParse<ELocation>(currentUser.Location, out var adminLocation) &&
                    user.Location != adminLocation)
                {
                    // Admin is trying to access a user from a different location
                    throw new AppException(ErrorCode.ACCESS_DENIED);
                }
            }

            return user.MapModelToResponse();
        }
    }
}
