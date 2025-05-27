// Updated UserService.cs
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
using Microsoft.AspNetCore.Identity;
using System.Globalization;
using System.Text;

namespace AssetManagement.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(
            IUserRepository userRepository,
            IAccountRepository accountRepository,
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _accountRepository = accountRepository;
            _unitOfWork = unitOfWork;
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

        public async Task<PagedList<UserResponse>> GetUsersAsync(UserParams userParams, string location, string currentUserStaffCode)
        {
            // First, get all users
            var query = _userRepository.GetAllAsync();

            // Exclude the currently logged in user
            query = query.Where(u => u.StaffCode != currentUserStaffCode);

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

        public async Task<UserResponse> CreateUserAsync(CreateUserRequest createUserRequest, string adminStaffCode)
        {
            // Get admin's location
            var admin = await _userRepository.GetByIdAsync(adminStaffCode);
            if (admin == null)
            {
                throw new AppException(ErrorCode.USER_NOT_FOUND);
            }

            // Generate staff code
            string staffCode = GenerateStaffCode();

            // Generate username and password
            (string username, string password) = GenerateCredentials(createUserRequest);

            // Create user entity
            var user = new User
            {
                StaffCode = staffCode,
                UserName = username,
                FirstName = createUserRequest.FirstName.Trim(),
                LastName = createUserRequest.LastName.Trim(),
                DateOfBirth = createUserRequest.DateOfBirth,
                JoinedDate = createUserRequest.JoinedDate,
                Gender = createUserRequest.Gender,
                Type = createUserRequest.Type,
                Location = admin.Location,
                IsDisabled = false,
                IsFirstLogin = true
            };

            // Create user in the repository
            await _userRepository.CreateAsync(user);

            // Create account
            var account = new Account
            {
                Id = Guid.NewGuid(),
                UserName = username,
                StaffCode = staffCode,
                CreatedDate = DateTime.Now
            };

            // Use the AccountRepository to create the account with password
            var result = await _accountRepository.CreateAccountAsync(account, password);
            if (!result.Succeeded)
            {
                // If account creation fails, delete the created user
                await _userRepository.DeleteAsync(user);

                var errors = string.Join(", ", result.Errors);
                throw new AppException(ErrorCode.IDENTITY_CREATION_FAILED, new Dictionary<string, object> { { "errors", errors } });
            }

            // Add user to appropriate role
            var roleResult = await _accountRepository.AddToRoleAsync(account, createUserRequest.Type.ToString());
            if (!roleResult.Succeeded)
            {
                // Handle role assignment failure
                await _userRepository.DeleteAsync(user);
                var errors = string.Join(", ", roleResult.Errors);
                throw new AppException(ErrorCode.IDENTITY_CREATION_FAILED, new Dictionary<string, object> { { "errors", errors } });
            }

            await _unitOfWork.CommitAsync();

            return user.MapModelToResponse();
        }

        private string GenerateStaffCode()
        {
            // Lấy tất cả người dùng
            var allUsers = _userRepository.GetAllAsync().ToList();
            int lastNumber = 0;

            if (allUsers.Any())
            {
                // Vì tất cả staff code đều bắt đầu bằng "SD", chúng ta có thể bỏ điều kiện lọc
                var codes = allUsers
                    .Select(u => u.StaffCode)
                    .Select(c =>
                    {
                        if (int.TryParse(c[2..], out int num))
                            return num;
                        return 0;
                    });

                if (codes.Any())
                    lastNumber = codes.Max();
            }

            int newNumber = lastNumber + 1;
            // Sử dụng chuỗi định dạng D4 để đảm bảo luôn có 4 chữ số (thêm số 0 ở đầu nếu cần)
            return $"SD{newNumber:D4}";
        }


        private (string username, string password) GenerateCredentials(CreateUserRequest request)
        {
            string firstName = request.FirstName.Trim();
            string lastName = request.LastName.Trim();

            // Tạo username: Lấy toàn bộ first name + chữ cái đầu tiên của mỗi từ trong last name
            StringBuilder usernameBuilder = new StringBuilder(firstName.ToLower());

            // Xử lý last name, lấy chữ cái đầu tiên của mỗi từ
            string[] lastNameParts = lastName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            foreach (var part in lastNameParts)
            {
                if (!string.IsNullOrEmpty(part))
                {
                    usernameBuilder.Append(part[0].ToString().ToLower());
                }
            }

            // Chuẩn hóa tên người dùng (loại bỏ dấu, khoảng trắng)
            string baseUsername = usernameBuilder.ToString()
                .Normalize(NormalizationForm.FormD)
                .Where(c => char.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                .Aggregate(new StringBuilder(), (sb, c) => sb.Append(c))
                .ToString()
                .Replace(" ", "");

            // Kiểm tra nếu username đã tồn tại và tạo username duy nhất
            string username = baseUsername;
            int counter = 1;

            while (_accountRepository.FindByUserNameAsync(username).Result != null)
            {
                username = $"{baseUsername}{counter}";
                counter++;
            }

            // Tạo mật khẩu [username]@[DOB in ddMMyyy]
            string password = $"{username}@{request.DateOfBirth:ddMMyyyy}";

            return (username, password);
        }

    }
}