using DemoMVC.Models.Entities;

namespace DemoMVC.Models.ViewModels;

public class ProductTableViewModel
{
    public IReadOnlyCollection<Product> Items { get; init; } = [];
    public string SearchTerm { get; init; } = string.Empty;
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public int TotalItems { get; init; }

    public int TotalPages => Math.Max(1, (int)Math.Ceiling(TotalItems / (double)PageSize));
}
