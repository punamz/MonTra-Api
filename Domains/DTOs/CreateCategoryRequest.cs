using MonTraApi.Common;

namespace MonTraApi.Domains.DTOs;

public class CreateCategoryRequest
{
    public CategoryType Type { get; set; } = CategoryType.Expenses;
    public string? Category { get; set; }
}
