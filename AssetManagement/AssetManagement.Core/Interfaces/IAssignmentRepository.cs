using AssetManagement.Core.Entities;
using AssetManagement.Core.Interfaces.Base;

namespace AssetManagement.Core.Interfaces
{
    interface IAssignmentRepository : IBaseRepository<Assignment>
    {
        Task<Assignment?> GetByIdAsync(int assignmentId);
    }
}
