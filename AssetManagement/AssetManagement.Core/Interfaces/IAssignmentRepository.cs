using AssetManagement.Core.Entities;
using AssetManagement.Core.Interfaces.Base;

namespace AssetManagement.Core.Interfaces
{
    public interface IAssignmentRepository : IBaseRepository<Assignment>
    {
        Task<Assignment?> GetByIdAsync(int assignmentId);
        Task<bool> IsUserInViewAssignments(string staffCode);
    }
}
