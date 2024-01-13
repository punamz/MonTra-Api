using MonTraApi.Domains.DTOs;
using System.Security.Cryptography;

namespace MonTraApi.Common;

public class Helper
{


    public static (string salt, string hashed) HashPassword(string password, string? lastSalt = null)
    {

        int saltSize = 128 / 8;
        int keySize = 256 / 8;
        int iterations = 100000;
        HashAlgorithmName hashAlgorithmName = HashAlgorithmName.SHA256;
        byte[] _salt = lastSalt == null ? RandomNumberGenerator.GetBytes(saltSize) : Convert.FromBase64String(lastSalt);
        byte[] _hash = Rfc2898DeriveBytes.Pbkdf2(password, _salt, iterations, hashAlgorithmName, keySize);
        return (lastSalt ?? Convert.ToBase64String(_salt), Convert.ToBase64String(_hash));
    }

    public static ResultDTO<T> GetResponse<T>(T? data = default, StatusCodeValue statusCode = StatusCodeValue.Success, string? errorCode = null, string? message = null)
    {
        return new ResultDTO<T>()
        {
            StatusCode = statusCode,
            Message = message,
            Data = data,
            ErrorCode = errorCode
        };
    }

}
