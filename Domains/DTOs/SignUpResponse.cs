using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MonTraApi.Domains.DTOs;


[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
public class SignUpResponse
{
    public string Token { get; set; } = null!;
    public UserDTO User { get; set; } = null!;
}
