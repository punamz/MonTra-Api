using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MonTraApi.Domains.DTOs;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
public class TransactionDTO
{
    public string Id { get; set; } = null!;
    public string UserId { get; set; } = null!;
    public int Amount { get; set; } = 0;
    public string? Description { get; set; }
    public DateTime TransactionAt { get; set; } = DateTime.Now;
    public CategoryDTO Category { get; set; } = new CategoryDTO();
}
