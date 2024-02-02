using MonTraApi.Common;

namespace MonTraApi.Domains.DTOs;

public class CreateCategoryRequest
{
    public string UserId { get; set; } = null!;
    public CategoryType Type { get; set; } = CategoryType.Expenses;
    public string? Category { get; set; }
    public string? Color { get; set; }
    public string? Icon { get; set; }
}
