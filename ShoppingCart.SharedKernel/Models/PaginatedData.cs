namespace ShoppingCart.SharedKerel.Models;

public class PaginatedData<T>
{
    public int TotalPages { get; set; }
    public int CurrentPage { get; set; }
    public int TotalItemsCount { get; set; }
    public int CurrentItemsCount { get; set; }
    public int ItemsPerPage { get; set; }
    public List<T> Items { get; set; } = new List<T>();
}