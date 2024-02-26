using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using MonTraApi.Common;
using MonTraApi.Domains.Entities;

namespace MonTraApi.Infrastructures.Queries;

public static class TransactionQuery
{
    /// <summary>
    /// get transaction entity
    /// </summary>
    /// <param name="collection"></param>
    /// <param name="userId"></param>
    /// <param name="limit"></param>
    /// <param name="offset"></param>
    /// <returns></returns>
    public static async Task<List<TransactionEntity>> GetTransactionByUserId(this IMongoCollection<TransactionEntity> collection, string userId, int limit, int offset)
    {
        List<TransactionEntity> transactions = await collection.Find(transaction => transaction.UserId == userId).Skip(offset).Limit(limit).ToListAsync();
        return transactions;
    }

    /// <summary>
    /// get transaction and join relative collection
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="limit"></param>
    /// <param name="offset"></param>
    /// <returns></returns>
    public static async Task<List<TransactionAggregate>> GetTransactionAggregate(this IMongoDatabase database, string userId, int limit, int offset, OrderByType OrderBy, CategoryType? categoryType = null, List<string>? categoriesId = null)
    {
        IMongoCollection<TransactionEntity> transactionCollection = database.TransactionColection();
        IMongoCollection<CategoryEntity> categoryCollection = database.CategoryColection();

        List<TransactionAggregate> transactions = await transactionCollection
            .Aggregate()
            .Match(x => x.UserId == userId)
            .Match(x => categoriesId == null || categoriesId.IsNullOrEmpty() || categoriesId.Contains(x.CategoryId))
            .Sort(OrderBy)
            .Lookup<TransactionEntity, CategoryEntity, TransactionAggregate>(categoryCollection, transactionEntity => transactionEntity.CategoryId, category => category.Id, transactionAggregate => transactionAggregate.Category)
            .Unwind(p => p.Category, new AggregateUnwindOptions<TransactionAggregate>() { PreserveNullAndEmptyArrays = true })
            .Match(x => categoriesId != null || categoryType == null || x.Category.Type == categoryType)
            .Skip(offset)
            .Limit(limit)
            .ToListAsync();
        return transactions;
    }
    private static IOrderedAggregateFluent<TransactionEntity> Sort(this IAggregateFluent<TransactionEntity> aggregate, OrderByType type)
    {
        return type switch
        {
            OrderByType.Highest => aggregate.SortByDescending(x => x.Amount),
            OrderByType.Lowest => aggregate.SortBy(x => x.Amount),
            OrderByType.Newest => aggregate.SortByDescending(x => x.TransactionAt),
            OrderByType.Oldest => aggregate.SortBy(x => x.TransactionAt),
            _ => aggregate.SortBy(x => x.TransactionAt),
        };
    }

    /// <summary>
    /// get all transaction for Frequency 
    /// </summary>
    /// <param name="database"></param>
    /// <param name="userId"></param>
    /// <param name="categoryType"></param>
    /// <param name="startTime"></param>
    /// <returns></returns>
    public static async Task<List<TransactionFrequencyEntity>> GetTransactionFrequency(this IMongoDatabase database, string userId, CategoryType? categoryType, DateTime startTime)
    {
        IMongoCollection<TransactionEntity> transactionCollection = database.TransactionColection();
        IMongoCollection<CategoryEntity> categoryCollection = database.CategoryColection();

        List<TransactionFrequencyEntity> transactions = await transactionCollection
             .Aggregate()
             .Match(x => x.UserId == userId && x.TransactionAt > startTime)
             .Lookup<TransactionEntity, CategoryEntity, TransactionAggregate>(categoryCollection, transactionEntity => transactionEntity.CategoryId, category => category.Id, transactionAggregate => transactionAggregate.Category)
             .Unwind(p => p.Category, new AggregateUnwindOptions<TransactionAggregate>() { PreserveNullAndEmptyArrays = true })
             .Match(x => x.Category.Type == categoryType)
             .Project(x => new TransactionFrequencyEntity()
             {
                 TransactionAt = x.TransactionAt,
                 Amount = x.Amount
             })
             .ToListAsync();
        return transactions;
    }

}
