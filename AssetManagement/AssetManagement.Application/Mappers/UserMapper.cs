﻿using AssetManagement.Application.DTOs.Users;
using AssetManagement.Core.Entities;

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
                Type = Enum.GetName(user.Type)!,
                Location = Enum.GetName(user.Location)!,
                IsDisabled = user.IsDisabled,
                IsFirstLogin = user.IsFirstLogin
            };
        }
    }
}
