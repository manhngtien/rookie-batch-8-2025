using AssetManagement.Core.Entities;
using AssetManagement.Core.Interfaces.Base;

namespace AssetManagement.Core.Interfaces
{
    public interface IReturningRequestRepository : IBaseRepository<ReturningRequest>
    {
        Task<ReturningRequest?> GetByIdAsync(int returningRequestId);
    }
}
