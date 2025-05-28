using AssetManagement.Application.DTOs.Users;
using AssetManagement.Application.Helpers.Params;
using AssetManagement.Application.Paginations;

namespace AssetManagement.Application.Interfaces
{
    public interface IUserService
    {
        Task<PagedList<UserResponse>> GetUsersAsync(UserParams userParams, string location, string currentUserStaffCode);
        Task<UserResponse> GetUserByIdAsync(string staffCode, string location);
        Task<string> GetLocationByStaffCodeAsync(string staffCode);
        Task<UserResponse> CreateUserAsync(CreateUserRequest createUserRequest, string adminStaffCode);
        Task<UserResponse> UpdateUserAsync(string staffCode, UpdateUserRequest updateUserRequest, string adminStaffCode);

    }
}