namespace AssetManagement.Application.DTOs.Accounts
{
    public class LoginRequest
    {
        public string UserName { get; set; }
        public required string Password { get; set; }
    }
}
