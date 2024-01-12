using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MonTraApi.Domains.Entities;

public class AccountEntity : Timestamp
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;
    [BsonElement("email")]
    public string Email { get; set; } = null!;
    [BsonElement("password")]
    public string Password { get; set; } = null!;
    [BsonElement("userId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string UserId { get; set; } = null!;
}
