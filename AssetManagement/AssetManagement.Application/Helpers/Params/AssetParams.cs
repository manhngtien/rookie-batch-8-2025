namespace AssetManagement.Application.Helpers.Params
{
    public class AssetParams : PaginationParams
    {
        public string? OrderBy { get; set; }
        public string? SearchTerm { get; set; }
        public string? Category { get; set; }
        public string? State { get; set; }
    }
}
