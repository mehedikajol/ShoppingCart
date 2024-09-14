namespace ShoppingCart.SharedKerel.Models;

public class PaginatedRequest
{
    public string? SearchProperty { get; set; }
    public string? SearchText { get; set; }
    public int CurrentPage { get; set; } = 1;
    public int ItemsPerPage { get; set; } = 5;
    public string? SortBy { get; set; }
    public int SortOrder { get; set; }
    public string[] NavigationProperties { get; set; } = [];
}
