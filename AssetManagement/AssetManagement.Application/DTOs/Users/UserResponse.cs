namespace AssetManagement.Application.DTOs.Users
{
    public class UserResponse
    {
        public required string StaffCode { get; set; }
        public required string UserName { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public required DateTime DateOfBirth { get; set; }
        public required DateTime JoinedDate { get; set; }
        public bool Gender { get; set; }
        public required string Type { get; set; }
        public required string Location { get; set; }
        public bool IsDisabled { get; set; }
        public bool IsFirstLogin { get; set; }
    }
}
