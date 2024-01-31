using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MonTraApi.Domains.DTOs;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
public class CreateNewTransactionRequest
{
    public string UserId { get; set; } = null!;
    public string CategoryId { get; set; } = null!;
    public int Amount { get; set; } = 0;
    public string? Note { get; set; }
    public DateTime TransactionAt { get; set; } = DateTime.Now;

}
