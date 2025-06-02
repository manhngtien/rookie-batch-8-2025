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
                AssetCode = assignment.AssetCode,
                AssignedDate = DateOnly.FromDateTime(assignment.AssignedDate),
                AssetName = assignment.Asset.AssetName,
                AssignedByUser = assignment.AssignedByUser.MapModelToResponse(),
                AssignedToUser = assignment.AssignedToUser.MapModelToResponse(),
                Note = assignment.Note ?? string.Empty
            };
        }
    }
}
