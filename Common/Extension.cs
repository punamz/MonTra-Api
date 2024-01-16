using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MonTraApi.Domains.Entities;
using System.Text.RegularExpressions;

namespace MonTraApi.Common;

public static partial class Extension
{
    public static IMongoCollection<UserEntity> UserColection(this IMongoDatabase database) => database.GetCollection<UserEntity>(ConstantValue.UserCollection);
    public static IMongoCollection<AccountEntity> AccountColection(this IMongoDatabase database) => database.GetCollection<AccountEntity>(ConstantValue.AccountCollection);

    public static bool IsEmail(this string value) => EmailRegex().IsMatch(value);
    public static bool ValidPassword(this string value) => !value.Contains(' ') && value.Length >= 8;
    public static bool ValidName(this string value)
    {
        value = BeautiRegex().Replace(value, " ");
        return value.Trim().Length > 0;
    }
    [GeneratedRegex("\\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\\Z", RegexOptions.IgnoreCase, "en-GB")]
    private static partial Regex EmailRegex();
    [GeneratedRegex("[ ]{2,}", RegexOptions.None)]
    private static partial Regex BeautiRegex();
}
