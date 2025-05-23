using AssetManagement.Application.DTOs.Assignments;
using AssetManagement.Core.Entities;

namespace AssetManagement.Application.Mappers
{
    public static class AssignmentMapper
    {
        public static AssignmentResponse MapModelToResponse(this Assignment assignment)
        {
            return new AssignmentResponse
            {
                Id = assignment.Id,
                State = assignment.State.ToString(),
                AssetCode = assignment.AssetCode ?? string.Empty,
                AssignedDate = DateOnly.FromDateTime(assignment.AssignedDate), // Chuyển từ DateTime sang DateOnly
                AssetName = assignment.Asset?.AssetName ?? string.Empty,
                AssignedTo = assignment.AssignedToUser?.UserName ?? string.Empty,
                AssignedBy = assignment.AssignedByUser?.UserName ?? string.Empty,
                Note = assignment.Note ?? string.Empty
            };
        }
    }
}
