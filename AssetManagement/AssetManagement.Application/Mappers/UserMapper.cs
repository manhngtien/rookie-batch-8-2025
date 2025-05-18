using AssetManagement.Application.DTOs.Users;
using AssetManagement.Core.Entities;
using AssetManagement.Core.Enums;

namespace AssetManagement.Application.Mappers
{
    public static class UserMapper
    {
        public static UserResponse MapModelToResponse(this User user)
        {
            return new UserResponse
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                StaffCode = user.StaffCode,
                DateOfBirth = user.DateOfBirth,
                Gender = user.Gender,
                JoinedDate = user.JoinedDate,
                Type = Enum.GetName(user.Type) ?? "Unknown",
                Location = Enum.GetName(user.Location) ?? "Unknown",
                IsDisabled = user.IsDisabled,
            };
        }
    }
}
