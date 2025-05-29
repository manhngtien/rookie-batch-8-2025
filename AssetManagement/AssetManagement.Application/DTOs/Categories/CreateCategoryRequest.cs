namespace AssetManagement.Application.DTOs.Categories;

public class CreateCategoryRequest
{
    public required string CategoryName { get; set; }
    public required string Prefix { get; set; }
}