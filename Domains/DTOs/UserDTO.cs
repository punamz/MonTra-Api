using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MonTraApi.Domains.DTOs;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
public class UserDTO
{
    public string Id { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string? Avatar { get; set; }

}
