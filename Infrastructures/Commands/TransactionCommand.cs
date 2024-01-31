using MongoDB.Driver;
using MonTraApi.Domains.Entities;

namespace MonTraApi.Infrastructures.Commands;

public static class TransactionCommand
{
    public static async Task CreateNewTransaction(this IMongoCollection<TransactionEntity> collection, TransactionEntity transaction)
    {
        await collection.InsertOneAsync(transaction);
    }
}
