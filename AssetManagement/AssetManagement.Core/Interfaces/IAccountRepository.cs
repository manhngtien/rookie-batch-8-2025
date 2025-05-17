using AssetManagement.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Core.Interfaces
{
    interface IAccountRepository
    {
        IQueryable<Account> GetAllAsync();
        Task<Account?> GetByIdAsync(Guid accountId);
        Task<List<Account>> GetAccountsInRoleAsync(string role);
        Task<bool> IsInRoleAsync(Account account, string role);
        Task<IList<string>> GetRolesAsync(Account account);
        Task<Account?> FindByEmailAsync(string email);
        Task<bool> CheckPasswordAsync(Account account, string password);
        Task UpdateAsync(Account account);
        Task<bool> CreateAsync(Account account, string password);
        Task AddToRoleAsync(Account account, string role);
        Task<Account?> FindByRefreshTokenAsync(string refreshToken);
    }
}
