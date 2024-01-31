using Microsoft.IdentityModel.Tokens;
using MonTraApi.Domains.DTOs;
using MonTraApi.Domains.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

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

    public static string CreateJWTToken(UserEntity user)
    {
        string jwtKey = Environment.GetEnvironmentVariable(ConstantValue.JWTKey) ?? "";
        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Subject = new ClaimsIdentity(new[]  {
                new Claim(ConstantValue.JWTUserIdKey, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            }),
            Expires = DateTime.UtcNow.AddMinutes(60),
            SigningCredentials = new SigningCredentials
           (new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtKey)), SecurityAlgorithms.HmacSha512Signature)
        };
        JwtSecurityTokenHandler tokenHandler = new();
        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
        string jwtToken = tokenHandler.WriteToken(token);
        return jwtToken;
    }
}
