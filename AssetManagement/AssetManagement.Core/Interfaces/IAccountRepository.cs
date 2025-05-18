using AssetManagement.Core.Entities;

namespace AssetManagement.Core.Interfaces
{
    public interface IAccountRepository
    {
        Task<Account?> GetByIdAsync(Guid accountId);
        Task<Account?> FindByUserNameAsync(string userName);
        Task<bool> CheckPasswordAsync(Account account, string password);
        Task<IList<string>> GetRolesAsync(Account account);
    }
}
