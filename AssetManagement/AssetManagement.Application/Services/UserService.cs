using AssetManagement.Application.DTOs.Paginations;
using AssetManagement.Application.DTOs.Users;
using AssetManagement.Application.Helpers.Params;
using AssetManagement.Application.Mappers;
using AssetManagement.Core.Exceptions;
using AssetManagement.Core.Interfaces;
using AssetManagement.Core.Interfaces.Services;
using AssetManagement.Infrastructure.Exceptions;
using AssetManagement.Infrastructure.Extensions;

namespace AssetManagement.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPaginationService _paginationService;

        public UserService(IUserRepository userRepository, IPaginationService paginationService)
        {
            _userRepository = userRepository;
            _paginationService = paginationService;
        }

        public async Task<PagedList<UserResponse>> GetUsersAsync(UserParams userParams)
        {
            var query = _userRepository.GetAllAsync()
               .Sort(userParams.OrderBy)
               .Search(userParams.SearchTerm)
               .Filter(userParams.Type);

            var projectedQuery = query.Select(x => x.MapModelToResponse());

            return await _paginationService.ToPagedList(
                projectedQuery,
                userParams.PageNumber,
                userParams.PageSize
            );
        }

        public async Task<UserResponse> GetUserByIdAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                var attributes = new Dictionary<string, object>
                {
                    { "userId", userId }
                };
                throw new AppException(ErrorCode.USER_NOT_FOUND, attributes);
            }

            return user.MapModelToResponse();
        }
    }
}
