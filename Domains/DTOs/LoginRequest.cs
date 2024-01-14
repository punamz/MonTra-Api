using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MonTraApi.Domains.DTOs;


[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
public class LoginRequest
{
    public string? Email { get; set; }
    public string? Password { get; set; }
}
