using AssetManagement.Core.Entities;
using AssetManagement.Core.Enums;
using AssetManagement.Core.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace AssetManagement.Infrastructure.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly List<Account> _mockAccounts;
        private readonly IPasswordHasher<Account> _passwordHasher;

        public AccountRepository()
        {
            _passwordHasher = new PasswordHasher<Account>();

            // Mock users to align with UserRepository
            var mockUsers = new List<User>
            {
                new User
                {
                    StaffCode = "SC001",
                    UserName = "john.doe",
                    FirstName = "John",
                    LastName = "Doe",
                    DateOfBirth = new DateTime(1990, 5, 15),
                    JoinedDate = new DateTime(2023, 1, 10),
                    Gender = true,
                    Type = ERole.Admin,
                    Location = ELocation.HN,
                    IsDisabled = false
                },
                new User
                {
                    StaffCode = "SC002",
                    UserName = "jane.smith",
                    FirstName = "Jane",
                    LastName = "Smith",
                    DateOfBirth = new DateTime(1988, 11, 22),
                    JoinedDate = new DateTime(2022, 6, 15),
                    Gender = false,
                    Type = ERole.Staff,
                    Location = ELocation.HCM,
                    IsDisabled = false
                },
                new User
                {
                    StaffCode = "SC003",
                    UserName = "alice.johnson",
                    FirstName = "Alice",
                    LastName = "Johnson",
                    DateOfBirth = new DateTime(1995, 3, 8),
                    JoinedDate = new DateTime(2024, 2, 20),
                    Gender = false,
                    Type = ERole.Staff,
                    Location = ELocation.HN,
                    IsDisabled = true
                }
            };

            _mockAccounts = new List<Account>
            {
                new Account
                {
                    Id = Guid.Parse("123e4567-e89b-12d3-a456-426614174000"),
                    UserName = "john.doe",
                    StaffCode = "SC001",
                    CreatedDate = DateTime.UtcNow.AddDays(-30),
                    User = mockUsers[0],
                    PasswordHash = _passwordHasher.HashPassword(null, "Password123!")
                },
                new Account
                {
                    Id = Guid.Parse("987fcdeb-12ab-34cd-5678-426614174001"),
                    UserName = "jane.smith",
                    StaffCode = "SC002",
                    CreatedDate = DateTime.UtcNow.AddDays(-20),
                    User = mockUsers[1],
                    PasswordHash = _passwordHasher.HashPassword(null, "Password456!")
                },
                new Account
                {
                    Id = Guid.Parse("abcdef12-3456-7890-abcd-426614174002"),
                    UserName = "alice.johnson",
                    StaffCode = "SC003",
                    CreatedDate = DateTime.UtcNow.AddDays(-10),
                    User = mockUsers[2],
                    PasswordHash = _passwordHasher.HashPassword(null, "Password789!")
                }
            };
        }

        public async Task<Account?> GetByIdAsync(Guid accountId)
        {
            var account = _mockAccounts.FirstOrDefault(a => a.Id == accountId);
            return await Task.FromResult(account);
        }

        public async Task<Account?> FindByUserNameAsync(string userName)
        {
            var account = _mockAccounts.FirstOrDefault(a => a.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase));
            return await Task.FromResult(account);
        }

        public async Task<bool> CheckPasswordAsync(Account account, string password)
        {
            if (account == null)
            {
                return false;
            }

            var result = _passwordHasher.VerifyHashedPassword(null, account.PasswordHash, password);
            return await Task.FromResult(result == PasswordVerificationResult.Success);
        }

        public async Task<IList<string>> GetRolesAsync(Account account)
        {
            if (account == null)
            {
                return new List<string>();
            }

            // Map ERole to string roles based on User.Type
            var roles = new List<string>();
            if (account.User != null)
            {
                switch (account.User.Type)
                {
                    case ERole.Admin:
                        roles.Add("Admin");
                        break;
                    case ERole.Staff:
                        roles.Add("Staff");
                        break;
                }
            }

            return await Task.FromResult(roles);
        }

        public async Task<Account?> GetByStaffCodeAsync(string staffCode)
        {
            var account = _mockAccounts.FirstOrDefault(a => a.StaffCode.Equals(staffCode, StringComparison.OrdinalIgnoreCase));
            return await Task.FromResult(account);
        }

        public async Task<bool> ChangePasswordAsync(Account account, string newPassword)
        {
            if (account == null)
            {
                return false;
            }

            var existingAccount = _mockAccounts.FirstOrDefault(a => a.Id == account.Id);
            if (existingAccount == null)
            {
                return false;
            }

            existingAccount.PasswordHash = _passwordHasher.HashPassword(null, newPassword);
            return await Task.FromResult(true);
        }
    }
}