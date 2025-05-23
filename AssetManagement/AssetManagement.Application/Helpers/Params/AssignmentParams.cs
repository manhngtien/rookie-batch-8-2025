using AssetManagement.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace AssetManagement.Application.Helpers.Params;


public class AssignmentParams : PaginationParams
{
    public string? SearchTerm { get; set; }
    [EnumDataType(typeof(AssignmentStatus), ErrorMessage = "Invalid state value.")]
    public string? State;
    public DateOnly? AssignedDate { get; set; }
    public string? OrderBy { get; set; }
}