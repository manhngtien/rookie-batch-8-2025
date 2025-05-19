using AssetManagement.Application.DTOs.Users;
using AssetManagement.Application.Helpers.Params;
using AssetManagement.Application.Paginations;

namespace AssetManagement.Application.Interfaces.User
{
    public interface IUserService
    {
        Task<PagedList<UserResponse>> GetUsersAsync(UserParams userParams, UserResponse currentUser);
        Task<UserResponse> GetUserByIdAsync(string staffCode,UserResponse currentUser);
    }
}