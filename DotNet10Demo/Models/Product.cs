namespace DotNet10Demo.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsDeleted { get; set; }
    public bool IsActive { get; set; }
    public ProductDetails Details { get; set; } = null!;
}

public class ProductDetails
{
    public string Description { get; set; } = string.Empty;
    public List<string> Tags { get; set; } = [];
}
