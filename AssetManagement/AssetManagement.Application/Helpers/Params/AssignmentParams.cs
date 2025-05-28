namespace AssetManagement.Application.Helpers.Params;

public class AssignmentParams : PaginationParams
{
    public string? SearchTerm { get; set; }
    public string? State { get; set; }
    public DateOnly? AssignedDate { get; set; }
    public string? OrderBy { get; set; }
}