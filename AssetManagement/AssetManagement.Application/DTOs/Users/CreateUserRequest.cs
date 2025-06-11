namespace AssetManagement.Application.DTOs.Users
{
    public class CreateUserRequest
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required DateTime DateOfBirth { get; set; }
        public bool Gender { get; set; }
        public required DateTime JoinedDate { get; set; }
        public required string Type { get; set; }
        public string? Location { get; set; }
    }
}
