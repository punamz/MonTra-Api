using MongoDB.Driver;
using MonTraApi.Common;
using MonTraApi.Domains.Entities;

namespace MonTraApi.Infrastructures.Queries;

public static class AccountQuery
{
    /// <summary>
    /// Get account by email and password by user input.
    /// Return an AccountEntity object if has an account that match email and the password is correct 
    /// </summary>

    public static async Task<AccountEntity?> GetAccountByEmailPassword(this IMongoCollection<AccountEntity> collection, string email, string password)
    {
        AccountEntity? account = await collection.Find(account => account.Email == email).FirstOrDefaultAsync();

        // return if not has account
        if (account == null) return null;

        //check password
        var element = account.Password.Split(ConstantValue.PasswordHashDelimiter);
        var (_, hashed) = Helper.HashPassword(password, element[0]);

        if (hashed == element[1])
            return account;

        return null;
    }

}
