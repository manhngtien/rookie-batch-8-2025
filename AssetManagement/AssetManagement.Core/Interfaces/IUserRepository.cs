using AssetManagement.Core.Entities;
using AssetManagement.Core.Interfaces.Base;

namespace AssetManagement.Core.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User?> GetByIdAsync(string userId);
    }
}
