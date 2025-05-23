using AssetManagement.Core.Entities;

namespace AssetManagement.Core.Interfaces
{
    public interface IAccountRepository
    {
        // AppDbContext
        Task<Account?> GetByIdAsync(Guid accountId);
        Task<Account?> GetByStaffCodeAsync(string staffCode);

        // UserManager
        Task<Account?> FindByUserNameAsync(string userName);
        Task<bool> CheckPasswordAsync(Account account, string password);
        Task<bool> ChangePasswordAsync(Account account, string newPassword);
        Task<IList<string>> GetRolesAsync(Account account);
    }
}
