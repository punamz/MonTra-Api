using MonTraApi.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MonTraApi.Domains.DTOs;


[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
public class GetFrequencyRequest
{
    public string UserId { get; set; } = null!;
    public int TimeZone { get; set; }
    public FrequencyType Type { get; set; }
    public CategoryType CategoryType { get; set; }

}
