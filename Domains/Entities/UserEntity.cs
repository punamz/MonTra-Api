using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MonTraApi.Domains.Entities;

public class UserEntity : Timestamp
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;
    [BsonElement("email")]
    public string Email { get; set; } = null!;
    [BsonElement("fullName")]
    public string FullName { get; set; } = null!;
    [BsonElement("avatar")]
    public string? Avatar { get; set; }
}
