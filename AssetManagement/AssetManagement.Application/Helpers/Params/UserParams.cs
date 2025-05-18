namespace AssetManagement.Application.Helpers.Params
{
    public class UserParams : PaginationParams
    {
        public string? OrderBy { get; set; }
        public string? SearchTerm { get; set; }
        public string? Type { get; set; }
    }
}
