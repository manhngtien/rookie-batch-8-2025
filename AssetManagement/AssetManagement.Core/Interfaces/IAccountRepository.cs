using AssetManagement.Core.Entities;

namespace AssetManagement.Core.Interfaces
{
    public interface IAccountRepository
    {
        // AppDbContext
        Task<Account?> GetByIdAsync(Guid accountId);
        Task<Account?> GetByStaffCodeAsync(string staffCode);
        IQueryable<Account> GetAllAccounts();
        Task<(bool Succeeded, IEnumerable<string> Errors)> CreateAccountAsync(Account account, string password);
        Task<(bool Succeeded, IEnumerable<string> Errors)> AddToRoleAsync(Account account, string role);
        Task<(bool Succeeded, IEnumerable<string> Errors)> RemoveFromRoleAsync(Account account, string role);

        // UserManager
        Task<Account?> FindByUserNameAsync(string userName);
        Task<bool> CheckPasswordAsync(Account account, string password);
        Task<bool> ChangePasswordAsync(Account account, string newPassword);
        Task<IList<string>> GetRolesAsync(Account account);

        
    }
}
