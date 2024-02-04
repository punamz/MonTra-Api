using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MonTraApi.Domains.DTOs;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
public class GetFrequencyResponse
{
    public List<int>? Frequency { get; set; }
}
