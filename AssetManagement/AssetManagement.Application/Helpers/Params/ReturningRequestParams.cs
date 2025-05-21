namespace AssetManagement.Application.Helpers.Params;

public class ReturningRequestParams : PaginationParams
{
    public string? OrderBy { get; set; }
    public string? SearchTerm { get; set; }
    public string? State { get; set; }
    public DateOnly? ReturnedDate { get; set; }
}