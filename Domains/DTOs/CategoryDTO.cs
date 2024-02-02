using MonTraApi.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MonTraApi.Domains.DTOs;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
public class CategoryDTO
{
    public CategoryType Type { get; set; } = CategoryType.Expenses;
    public string Id { get; set; } = null!;
    public string? Category { get; set; }
    public string? Color { get; set; }
    public string? Icon { get; set; }
}
