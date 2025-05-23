using AssetManagement.Application.DTOs.Assignments;
using AssetManagement.Application.Helpers.Params;
using AssetManagement.Application.Paginations;


namespace AssetManagement.Application.Interfaces.Assignment
{
    public interface IAssignmentService
    {
        Task<PagedList<AssignmentResponse>> GetAssignmentsAsync(AssignmentParams assignmentParams);
        Task<AssignmentResponse> GetAssignmentByIdAsync(int id);

    }

}
