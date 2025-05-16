using Microsoft.AspNetCore.Identity;

namespace AssetManagement.Core.Entities
{
    public class Account : IdentityUser<Guid>
    {
        public override Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        
        // Foreign key
        public string StaffCode { get; set; }
        
        // Navigation property
        public virtual User User { get; set; } = null!;
    }
}
