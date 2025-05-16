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
        Task<Account?> GetByIdAsync(Guid userId);
        Task<List<Account>> GetUsersInRoleAsync(string role);
        Task<bool> IsInRoleAsync(Account user, string role);
        Task<IList<string>> GetRolesAsync(Account user);
        Task<Account?> FindByEmailAsync(string email);
        Task<bool> CheckPasswordAsync(Account user, string password);
        Task UpdateAsync(Account user);
        Task<bool> CreateAsync(Account user, string password);
        Task AddToRoleAsync(Account user, string role);
        Task<Account?> FindByRefreshTokenAsync(string refreshToken);
    }
}
