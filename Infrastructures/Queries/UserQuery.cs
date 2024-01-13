using MongoDB.Driver;
using MonTraApi.Domains.Entities;

namespace MonTraApi.Infrastructures.Queries;

public static class UserQuery
{
    public static async Task<UserEntity?> GetUserById(this IMongoCollection<UserEntity> collection, string id)
    {
        UserEntity? user = await collection.Find(user => user.Id == id).FirstOrDefaultAsync();
        return user;
    }
}
