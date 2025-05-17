using AssetManagement.Core.Entities;
using AssetManagement.Core.Interfaces.Repositories.Base;

namespace AssetManagement.Core.Interfaces.Repositories
{
    interface IAssignmentRepository : IBaseRepository<Assignment>
    {
        Task<Assignment?> GetByIdAsync(int assignmentId);
    }
}
