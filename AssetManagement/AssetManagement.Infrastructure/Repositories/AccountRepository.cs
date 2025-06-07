using AssetManagement.Core.Entities;
using AssetManagement.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace AssetManagement.Infrastructure.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly AppDbContext _context;
        private readonly UserManager<Account> _userManager;

        public AccountRepository(AppDbContext context, UserManager<Account> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<bool> ChangePasswordAsync(Account account, string newPassword)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(account);
            var result = await _userManager.ResetPasswordAsync(account, token, newPassword);
            return result.Succeeded;
        }

        public async Task<bool> CheckPasswordAsync(Account account, string password)
        {
            return await _userManager.CheckPasswordAsync(account, password);
        }

        public async Task<Account?> FindByUserNameAsync(string userName)
        {
            return await _userManager.FindByNameAsync(userName);
        }

        public IQueryable<Account> GetAllAccounts()
        {
            return _context.Users.AsNoTracking();
        }

        public async Task<Account?> GetByIdAsync(Guid accountId)
        {
            return await _context.Users
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == accountId);
        }

        public async Task<Account?> GetByStaffCodeAsync(string staffCode)
        {
            return await _context.Users
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.StaffCode == staffCode);
        }

        public async Task<IList<string>> GetRolesAsync(Account account)
        {
            return await _userManager.GetRolesAsync(account);
        }

        public async Task<(bool Succeeded, IEnumerable<string> Errors)> CreateAccountAsync(Account account, string password)
        {
            var result = await _userManager.CreateAsync(account, password);
            return (result.Succeeded, result.Errors.Select(e => e.Description));
        }

        public async Task<(bool Succeeded, IEnumerable<string> Errors)> AddToRoleAsync(Account account, string role)
        {
            var result = await _userManager.AddToRoleAsync(account, role);
            return (result.Succeeded, result.Errors.Select(e => e.Description));
        }

        public async Task<(bool Succeeded, IEnumerable<string> Errors)> RemoveFromRoleAsync(Account account, string role)
        {
            var result = await _userManager.RemoveFromRoleAsync(account, role);
            return (result.Succeeded, result.Errors.Select(e => e.Description));
        }

    }
}