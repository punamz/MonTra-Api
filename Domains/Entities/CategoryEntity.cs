using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MonTraApi.Common;

namespace MonTraApi.Domains.Entities;

public class CategoryEntity : Timestamp
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;
    [BsonElement("type")]
    public CategoryType Type { get; set; }
    [BsonElement("category")]
    public string? Category { get; set; }
    [BsonElement("color")]
    public string? Color { get; set; }
    [BsonElement("icon")]
    public string? Icon { get; set; }
    [BsonElement("userId")]
    public string UserId { get; set; } = null!;
}