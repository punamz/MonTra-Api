using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MonTraApi.Domains.Entities;

public class TransactionEntity : Timestamp
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;
    [BsonElement("categoryId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string CategoryId { get; set; } = null!;
    [BsonElement("userId")]
    public string UserId { get; set; } = null!;
    [BsonElement("amount")]
    public int Amount { get; set; } = 0;
    [BsonElement("description")]
    public string? Description { get; set; }
    [BsonElement("transactionAt")]
    public DateTime TransactionAt { get; set; } = DateTime.Now;
}
