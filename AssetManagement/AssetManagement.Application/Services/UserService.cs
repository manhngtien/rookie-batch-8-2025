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
using System.Security;
using System.Text;

namespace AssetManagement.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(
            IUserRepository userRepository,
            IAccountRepository accountRepository,
            IAssignmentRepository assignmentRepository,
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _accountRepository = accountRepository;
            _assignmentRepository = assignmentRepository;
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

            query = query.Where(u => !u.IsDisabled);

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

            // Convert Type from string to enum
            if (!Enum.TryParse<ERole>(createUserRequest.Type, true, out var roleType))
            {
                throw new AppException(ErrorCode.VALIDATION_ERROR, new Dictionary<string, object>
                {
            { "type", $"Invalid role type: {createUserRequest.Type}. Valid values are: {string.Join(", ", Enum.GetNames<ERole>())}" }
                });
            }

            ELocation userLocation;

            if (roleType == ERole.Admin && !string.IsNullOrEmpty(createUserRequest.Location))
            {
                // For Admin users, use the selected location
                if (!Enum.TryParse<ELocation>(createUserRequest.Location, true, out userLocation))
                {
                    throw new AppException(ErrorCode.VALIDATION_ERROR, new Dictionary<string, object>
            {
                { "location", $"Invalid location: {createUserRequest.Location}. Valid values are: {string.Join(", ", Enum.GetNames<ELocation>())}" }
            });
                }
            }
            else
            {
                userLocation = admin.Location;
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
                Type = roleType,
                Location = userLocation, 
                IsDisabled = false,
                IsFirstLogin = true
            };
           
            await _userRepository.CreateAsync(user);

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
            var roleResult = await _accountRepository.AddToRoleAsync(account, roleType.ToString());
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
            var allUsers = _userRepository.GetAllAsync().ToList();
            int lastNumber = 0;

            if (allUsers.Any())
            {
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
           
            return $"SD{newNumber:D4}";
        }


        private (string username, string password) GenerateCredentials(CreateUserRequest request)
        {
            string firstName = request.FirstName.Trim();
            string lastName = request.LastName.Trim();

            StringBuilder usernameBuilder = new StringBuilder(firstName.ToLower());

           
            string[] lastNameParts = lastName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            foreach (var part in lastNameParts)
            {
                if (!string.IsNullOrEmpty(part))
                {
                    usernameBuilder.Append(part[0].ToString().ToLower());
                }
            }

            string baseUsername = usernameBuilder.ToString()
                .Normalize(NormalizationForm.FormD)
                .Where(c => char.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                .Aggregate(new StringBuilder(), (sb, c) => sb.Append(c))
                .ToString()
                .Replace(" ", "");

            string username = baseUsername;
            int counter = 1;

            while (_accountRepository.FindByUserNameAsync(username).Result != null)
            {
                username = $"{baseUsername}{counter}";
                counter++;
            }

            string password = $"{username}@{request.DateOfBirth:ddMMyyyy}";

            return (username, password);
        }

        public async Task<UserResponse> UpdateUserAsync(string staffCode, UpdateUserRequest updateUserRequest, string adminStaffCode)
        {
            // Kiểm tra admin
            var admin = await _userRepository.GetByIdAsync(adminStaffCode);
            if (admin == null)
            {
                throw new AppException(ErrorCode.USER_NOT_FOUND);
            }

            // Lấy thông tin user cần cập nhật
            var user = await _userRepository.GetByIdAsync(staffCode);
            if (user == null)
            {
                throw new AppException(ErrorCode.USER_NOT_FOUND, new Dictionary<string, object> { { "staffCode", staffCode } });
            }

            // Kiểm tra xem admin có cùng location với user không
            if (admin.Location != user.Location)
            {
                throw new AppException(ErrorCode.ACCESS_DENIED);
            }

            // Chuyển đổi Type từ string sang enum
            if (!Enum.TryParse<ERole>(updateUserRequest.Type, true, out var roleType))
            {
                throw new AppException(ErrorCode.VALIDATION_ERROR, new Dictionary<string, object>
                {
                    { "type", $"Invalid role type: {updateUserRequest.Type}. Valid values are: {string.Join(", ", Enum.GetNames<ERole>())}" }
                });
            }

            // Cập nhật thông tin user
            user.DateOfBirth = updateUserRequest.DateOfBirth;
            user.Gender = updateUserRequest.Gender;
            user.JoinedDate = updateUserRequest.JoinedDate;
            user.Type = roleType;

            // Cập nhật user trong repository
            await _userRepository.UpdateAsync(user);

            // Cập nhật UpdatedDate trong Account
            var account = await _accountRepository.GetByStaffCodeAsync(staffCode);
            if (account != null)
            {
                account.UpdatedDate = DateTime.Now;

                // Cập nhật role nếu Type thay đổi
                var currentRoles = await _accountRepository.GetRolesAsync(account);
                if (!currentRoles.Contains(roleType.ToString()))
                {
                    // Xóa tất cả roles hiện tại và thêm role mới
                    foreach (var role in currentRoles)
                    {
                        await _accountRepository.RemoveFromRoleAsync(account, role);
                    }
                    await _accountRepository.AddToRoleAsync(account, roleType.ToString());
                }
            }

            await _unitOfWork.CommitAsync();

            return user.MapModelToResponse();
        }

        public async Task DisableUserAsync(string staffCode)
        {
            var user = await _userRepository.GetByIdAsync(staffCode);
            if (user == null)
            {
                throw new AppException(ErrorCode.USER_NOT_FOUND, new Dictionary<string, object>
                { 
                    { "staffCode", staffCode }
                });
            }

            if(await _assignmentRepository.IsUserInViewAssignments(staffCode))
            {
                throw new AppException(ErrorCode.USER_HAS_ACTIVE_ASSIGNMENTS, new Dictionary<string, object>
                {
                    { "staffCode", staffCode }
                });
            }

            user.IsDisabled = true;

            await _userRepository.UpdateAsync(user);

            await _unitOfWork.CommitAsync();
        }
    }
}