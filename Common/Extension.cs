using MongoDB.Driver;
using MonTraApi.Domains.Entities;

namespace MonTraApi.Common;

public static class Extension
{
    public static IMongoCollection<UserEntity> UserColection(this IMongoDatabase database) => database.GetCollection<UserEntity>(ConstantValue.UserCollection);
    public static IMongoCollection<AccountEntity> AccountColection(this IMongoDatabase database) => database.GetCollection<AccountEntity>(ConstantValue.AccountCollection);

}
