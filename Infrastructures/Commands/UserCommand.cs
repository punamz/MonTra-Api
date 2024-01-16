using MongoDB.Driver;
using MonTraApi.Domains.Entities;

namespace MonTraApi.Infrastructures.Commands;

public static class UserCommand
{
    /// <summary>
    /// Create new user
    /// </summary>
    public static async Task CreateNewUser(this IMongoCollection<UserEntity> collection, UserEntity user)
    {
        await collection.InsertOneAsync(user);
    }

}
