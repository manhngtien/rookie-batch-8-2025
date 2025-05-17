namespace AssetManagement.Core.Entities;

public class Category
{
    public int Id { get; set; }
    public required string Prefix { get; set; }
    public required string CategoryName { get; set; }
    public int Total { get; set; }

    // Navigation property
    public virtual ICollection<Asset> Assets { get; set; } = [];
}