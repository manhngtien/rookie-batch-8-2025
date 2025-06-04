using AssetManagement.Application.DTOs.Assignments;
using AssetManagement.Application.Helpers.Params;
using AssetManagement.Application.Paginations;


namespace AssetManagement.Application.Interfaces
{
	public interface IAssignmentService
	{
		Task<PagedList<AssignmentResponse>> GetAssignmentsAsync(string staffCode, AssignmentParams assignmentParams);
		Task<PagedList<AssignmentResponse>> GetAssignmentsByStaffCodeAsync(string staffCode, AssignmentParams assignmentParams);
		Task<AssignmentResponse> GetAssignmentByIdAsync(int id);
		Task<AssignmentResponse> CreateAssignmentAsync(string adminStaffCode, CreateAssignmentRequest assignmentRequest);
		Task<AssignmentResponse> UpdateAssignmentAsync(int id, string staffCode, UpdateAssignmentRequest assignmentRequest);
		Task ReplyAssignmentAsync(string staffCode, ReplyAssignmentRequest replyAssignmentRequest);
		Task DeleteAssignmentAsync(string staffCode, int assignmentId);
	}
}
