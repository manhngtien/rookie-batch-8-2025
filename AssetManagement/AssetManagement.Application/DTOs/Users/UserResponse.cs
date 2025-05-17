using AssetManagement.Core.Enums;

namespace AssetManagement.Application.DTOs.Users
{
    public class UserResponse
    {
        public required string StaffCode { get; set; }
        public required string UserName { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required DateTime DateOfBirth { get; set; }
        public required DateTime JoinedDate { get; set; }
        public bool Gender { get; set; }
        public ERole Type { get; set; }
        public required ELocation Location { get; set; }
        public bool IsDisabled { get; set; }
    }
}
