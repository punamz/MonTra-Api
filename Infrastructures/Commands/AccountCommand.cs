using MongoDB.Driver;
using MonTraApi.Domains.Entities;
using System;

namespace MonTraApi.Infrastructures.Commands;

public static class AccountCommand
{
    /// <summary>
    /// Create new Account
    /// </summary>
    public static async Task CreateNewAccount(this IMongoCollection<AccountEntity> collection, AccountEntity account)
    {
        await collection.InsertOneAsync(account);
    }

}
