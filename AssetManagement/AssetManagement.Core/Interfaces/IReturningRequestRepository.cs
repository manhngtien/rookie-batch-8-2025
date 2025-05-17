using AssetManagement.Core.Entities;
using AssetManagement.Core.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Core.Interfaces
{
    public interface IReturningRequestRepository : IBaseRepository<ReturningRequest>
    {
        Task<ReturningRequest?> GetByIdAsync(int returningRequestId);
    }
}
