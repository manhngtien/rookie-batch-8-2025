// Updated UserService.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AssetManagement.Application.DTOs.Users;
using AssetManagement.Application.Helpers.Params;
using AssetManagement.Application.Interfaces;
using AssetManagement.Application.Mappers;
using AssetManagement.Application.Paginations;
using AssetManagement.Core.Entities;
using AssetManagement.Core.Enums;
using AssetManagement.Core.Exceptions;
using AssetManagement.Core.Interfaces;
using AssetManagement.Infrastructure.Exceptions;
using AssetManagement.Infrastructure.Extensions;

namespace AssetManagement.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<string> GetLocationByStaffCodeAsync(string staffCode)
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

            return user.Location.ToString();
        }

        public async Task<PagedList<UserResponse>> GetUsersAsync(UserParams userParams, string location)
        {
            // First, get all users
            var query = _userRepository.GetAllAsync();

            // Filter users by location (admin can only see users from their location)
            if (Enum.TryParse<ELocation>(location, out var userLocation))
            {
                query = query.Where(u => u.Location == userLocation);
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

        public async Task<UserResponse> GetUserByIdAsync(string staffCode, string location)
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

            // Check if admin has the same location as the requested user
            if (Enum.TryParse<ELocation>(location, out var adminLocation) &&
                user.Location != adminLocation)
            {
                // Admin is trying to access a user from a different location
                throw new AppException(ErrorCode.ACCESS_DENIED);
            }

            return user.MapModelToResponse();
        }
    }
}
