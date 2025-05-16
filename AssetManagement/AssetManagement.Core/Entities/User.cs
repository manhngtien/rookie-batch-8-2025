using AssetManagement.Core.Enums;

namespace AssetManagement.Core.Entities
{
    public class User
    {
        public string StaffCode { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime JoinedDate { get; set; }
        public bool Gender { get; set; }
        public ERole Type { get; set; }
        public ELocation Location { get; set; }
        public bool IsDisabled { get; set; }
    }
}
