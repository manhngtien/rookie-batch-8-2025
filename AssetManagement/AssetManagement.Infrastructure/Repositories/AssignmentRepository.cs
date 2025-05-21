using AssetManagement.Core.Entities;
using AssetManagement.Core.Enums;
using AssetManagement.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetManagement.Infrastructure.Repositories
{
    public class AssignmentRepository : IAssignmentRepository
    {
        private readonly List<Assignment> _mockAssignments;
        private readonly List<User> _mockUsers;
        private readonly List<Asset> _mockAssets;

        public AssignmentRepository()
        {
            // Mock users (aligned with UserRepository and AccountRepository)
            _mockUsers = new List<User>
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

            // Mock assets
            _mockAssets = new List<Asset>
            {
                new Asset
                {
                    AssetCode = "PC001",
                    AssetName = "Laptop Dell XPS",
                    InstalledDate = new DateTime(2022, 1, 10),
                    State = AssetStatus.Available,
                    Location = ELocation.HN,
                    Specification = "16GB RAM, 512GB SSD"
                },
                new Asset
                {
                    AssetCode = "PC002",
                    AssetName = "Monitor LG 27",
                    InstalledDate = new DateTime(2021, 6, 15),
                    State = AssetStatus.Available,
                    Location = ELocation.HCM,
                    Specification = "4K, 60Hz"
                },
                new Asset
                {
                    AssetCode = "PC003",
                    AssetName = "Keyboard Logitech",
                    InstalledDate = new DateTime(2023, 3, 20),
                    State = AssetStatus.NotAvailable,
                    Location = ELocation.HN,
                    Specification = "Mechanical, RGB"
                }
            };

            // Mock assignments
            _mockAssignments = new List<Assignment>
            {
                new Assignment
                {
                    Id = 1,
                    AssetCode = "PC001",
                    Asset = _mockAssets[0],
                    AssignedTo = _mockUsers[1].StaffCode, // Jane Smith
                    AssignedToUser = _mockUsers[1],
                    AssignedBy = _mockUsers[0].StaffCode, // John Doe (Admin)
                    AssignedByUser = _mockUsers[0],
                    AssignedDate = new DateTime(2023, 10, 15),
                    State = AssignmentStatus.Accepted,
                    Note = "Assigned for development work"
                },
                new Assignment
                {
                    Id = 2,
                    AssetCode = "PC002",
                    Asset = _mockAssets[1],
                    AssignedTo = _mockUsers[2].StaffCode, // Alice Johnson
                    AssignedToUser = _mockUsers[2],
                    AssignedBy = _mockUsers[0].StaffCode, // John Doe (Admin)
                    AssignedByUser = _mockUsers[0],
                    AssignedDate = new DateTime(2024, 2, 20),
                    State = AssignmentStatus.WaitForAcceptance,
                    Note = "Pending acceptance"
                },
                new Assignment
                {
                    Id = 3,
                    AssetCode = "PC003",
                    Asset = _mockAssets[2],
                    AssignedTo = _mockUsers[1].StaffCode, // Jane Smith
                    AssignedToUser = _mockUsers[1],
                    AssignedBy = _mockUsers[0].StaffCode, // John Doe (Admin)
                    AssignedByUser = _mockUsers[0],
                    AssignedDate = new DateTime(2023, 12, 10),
                    State = AssignmentStatus.Accepted, // Changed from WaitForAcceptance for variety
                    Note = "Assigned for testing"
                }
            };
        }

        public async Task<Assignment> CreateAsync(Assignment entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            entity.Id = _mockAssignments.Any() ? _mockAssignments.Max(a => a.Id) + 1 : 1;
            _mockAssignments.Add(entity);
            return await Task.FromResult(entity);
        }

        public async Task DeleteAsync(Assignment entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var assignment = _mockAssignments.FirstOrDefault(a => a.Id == entity.Id);
            if (assignment == null)
                throw new InvalidOperationException($"Assignment with ID {entity.Id} not found.");

            _mockAssignments.Remove(assignment);
            await Task.CompletedTask;
        }

        public IQueryable<Assignment> GetAllAsync()
        {
            return _mockAssignments.AsQueryable();
        }

        public async Task<Assignment?> GetByIdAsync(int assignmentId)
        {
            var assignment = _mockAssignments.FirstOrDefault(a => a.Id == assignmentId);
            return await Task.FromResult(assignment);
        }

        public async Task<Assignment?> GetByIdAsync<TId>(TId id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            Assignment? assignment = null;
            if (id is int intId)
            {
                assignment = _mockAssignments.FirstOrDefault(a => a.Id == intId);
            }

            return await Task.FromResult(assignment);
        }

        public async Task UpdateAsync(Assignment entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var existingAssignment = _mockAssignments.FirstOrDefault(a => a.Id == entity.Id);
            if (existingAssignment == null)
                throw new InvalidOperationException($"Assignment with ID {entity.Id} not found.");

            existingAssignment.AssetCode = entity.AssetCode ?? existingAssignment.AssetCode;
            existingAssignment.Asset = entity.Asset ?? existingAssignment.Asset;
            existingAssignment.AssignedToUser = entity.AssignedToUser ?? existingAssignment.AssignedToUser;
            existingAssignment.AssignedByUser = entity.AssignedByUser ?? existingAssignment.AssignedByUser;
            existingAssignment.AssignedDate = entity.AssignedDate != default ? entity.AssignedDate : existingAssignment.AssignedDate;
            existingAssignment.State = entity.State != default ? entity.State : existingAssignment.State;
            existingAssignment.Note = entity.Note ?? existingAssignment.Note;

            await Task.CompletedTask;
        }
    }
}