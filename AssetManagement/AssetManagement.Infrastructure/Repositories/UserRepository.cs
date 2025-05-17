using AssetManagement.Core.Entities;
using AssetManagement.Core.Enums;
using AssetManagement.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetManagement.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly List<User> _mockUsers = new List<User>
        {
            new User
            {
                StaffCode = "SC001",
                UserName = "john.doe",
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = new DateTime(1990, 5, 15),
                JoinedDate = new DateTime(2023, 1, 10),
                Gender = true, // Male
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
                Gender = false, // Female
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
                Gender = false, // Female
                Type = ERole.Staff,
                Location = ELocation.HN,
                IsDisabled = true
            }
        };

        public async Task<User> CreateAsync(User entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var mockUser = new User
            {
                StaffCode = entity.StaffCode ?? $"SC{(_mockUsers.Count + 1):D03}",
                UserName = entity.UserName ?? $"user{_mockUsers.Count + 1}",
                FirstName = entity.FirstName ?? "New",
                LastName = entity.LastName ?? "User",
                DateOfBirth = entity.DateOfBirth != default ? entity.DateOfBirth : DateTime.UtcNow.AddYears(-30),
                JoinedDate = entity.JoinedDate != default ? entity.JoinedDate : DateTime.UtcNow,
                Gender = entity.Gender,
                Type = entity.Type != default ? entity.Type : ERole.Staff,
                Location = entity.Location != default ? entity.Location : ELocation.HN,
                IsDisabled = entity.IsDisabled
            };

            // Add to mock list to simulate persistence
            _mockUsers.Add(mockUser);

            return await Task.FromResult(mockUser);
        }

        public IQueryable<User> GetAllAsync()
        {
            // Return the mock user list as IQueryable
            return _mockUsers.AsQueryable();
        }

        public Task DeleteAsync(User entity)
        {
            throw new NotImplementedException();
        }

        public Task<User?> GetByIdAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<User?> GetByIdAsync<TId>(TId id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(User entity)
        {
            throw new NotImplementedException();
        }
    }
}