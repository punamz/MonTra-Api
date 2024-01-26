using MonTraApi.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MonTraApi.Domains.DTOs;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
public class ResultDTO<T>
{
    public StatusCodeValue StatusCode { get; set; } = StatusCodeValue.Success;
    public string? ErrorCode { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }

}
