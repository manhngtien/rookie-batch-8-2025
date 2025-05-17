using AssetManagement.Application.DTOs.Users;
using AssetManagement.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                Type = user.Type,
                Location = user.Location,
                IsDisabled = user.IsDisabled,
            };
        }
    }
}
