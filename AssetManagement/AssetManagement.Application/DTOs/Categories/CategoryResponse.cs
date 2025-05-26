namespace AssetManagement.Application.DTOs.Categories;

public class CategoryResponse
{
    public int Id { get; set; }
    public required string Prefix { get; set; }
    public required string CategoryName { get; set; }
    public int Total { get; set; }
}