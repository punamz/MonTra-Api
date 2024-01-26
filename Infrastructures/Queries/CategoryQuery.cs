using MongoDB.Driver;
using MonTraApi.Common;
using MonTraApi.Domains.Entities;

namespace MonTraApi.Infrastructures.Queries;

public static class CategoryQuery
{
    public static async Task<List<CategoryEntity>> GetCategoryByType(this IMongoCollection<CategoryEntity> collection, CategoryType type)
    {
        List<CategoryEntity> categories = await collection.Find(category => category.Type == type).ToListAsync();
        return categories;
    }

    public static async Task<List<CategoryEntity>> GetCategory(this IMongoCollection<CategoryEntity> collection, int limit, int offset)
    {
        List<CategoryEntity> categories = await collection.Find(_ => true).Skip(offset).Limit(limit).ToListAsync();
        return categories;
    }
}
