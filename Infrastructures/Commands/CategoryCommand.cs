using MongoDB.Driver;
using MonTraApi.Domains.Entities;

namespace MonTraApi.Infrastructures.Commands;

public static class CategoryCommand
{
    /// <summary>
    /// insert new category
    /// </summary>
    public static async Task CreateNewCategory(this IMongoCollection<CategoryEntity> collection, CategoryEntity account)
    {
        await collection.InsertOneAsync(account);
    }
}
