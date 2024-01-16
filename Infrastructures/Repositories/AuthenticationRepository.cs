using AutoMapper;
using MongoDB.Bson;
using MongoDB.Driver;
using MonTraApi.Common;
using MonTraApi.Domains.DTOs;
using MonTraApi.Domains.Entities;
using MonTraApi.Domains.Services;
using MonTraApi.Infrastructures.Commands;
using MonTraApi.Infrastructures.Queries;
using System.Diagnostics;
using System.Text.RegularExpressions;
namespace MonTraApi.Infrastructures.Repositories;

public class AuthenticationRepository : IAuthenticationService
{
    private readonly IMongoDatabase _database;
    private readonly IMapper _mapper;

    public AuthenticationRepository(IMongoDatabase database, IMapper mapper)
    {
        _database = database;
        _mapper = mapper;
    }

    public async Task<ResultDTO<LoginResponse>> Login(LoginRequest param)
    {
        try
        {
            // check valid param
            if (param.Email == null || param.Password == null)
                return Helper.GetResponse<LoginResponse>(statusCode: StatusCodeValue.Fail, errorCode: ConstantValue.Err0002, message: ConstantValue.Err0002Message);
            if (!param.Email.IsEmail())
            {
                return Helper.GetResponse<LoginResponse>(statusCode: StatusCodeValue.Fail, errorCode: ConstantValue.Err1003, message: ConstantValue.Err1003Message);
            } 
            if (!param.Password.ValidPassword())
            {
                return Helper.GetResponse<LoginResponse>(statusCode: StatusCodeValue.Fail, errorCode: ConstantValue.Err1004, message: ConstantValue.Err1004Message);
            }

            AccountEntity? account = await _database.AccountColection().GetAccountByEmailPassword(param.Email, param.Password);

            if (account == null)
                return Helper.GetResponse<LoginResponse>(statusCode: StatusCodeValue.NoData, errorCode: ConstantValue.Err1001, message: ConstantValue.Err1001Message);

            UserEntity? user = await _database.UserColection().GetUserById(account.UserId);
            if (user == null)
                return Helper.GetResponse<LoginResponse>(statusCode: StatusCodeValue.NoData, errorCode: ConstantValue.Err1002, message: ConstantValue.Err1002Message);

            // gen authen token
            string token = Helper.CreateJWTToken(user);
            LoginResponse res = new() { User = _mapper.Map<UserEntity, UserDTO>(user), Token = token };
            return Helper.GetResponse(data: res);
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
            return Helper.GetResponse<LoginResponse>(statusCode: StatusCodeValue.Fail, errorCode: ConstantValue.Err0001, message: $"Error: {e.Message}");
        }
    }

    public async Task<ResultDTO<SignUpResponse>> Registration(SignUpRequest param)
    {
        try
        {
            // check valid param
            if (param.Email == null || param.Password == null || param.FullName == null)
                return Helper.GetResponse<SignUpResponse>(statusCode: StatusCodeValue.Fail, errorCode: ConstantValue.Err0002, message: ConstantValue.Err0002Message);
            if (!param.Email.IsEmail())
            {
                return Helper.GetResponse<SignUpResponse>(statusCode: StatusCodeValue.Fail, errorCode: ConstantValue.Err1003, message: ConstantValue.Err1003Message);
            }
            if (!param.Password.ValidPassword())
            {
                return Helper.GetResponse<SignUpResponse>(statusCode: StatusCodeValue.Fail, errorCode: ConstantValue.Err1004, message: ConstantValue.Err1004Message);
            }
            if (!param.Password.ValidName())
            {
                return Helper.GetResponse<SignUpResponse>(statusCode: StatusCodeValue.Fail, errorCode: ConstantValue.Err1005, message: ConstantValue.Err1005Message);
            }

            AccountEntity? account = await _database.AccountColection().GetAccountByEmail(param.Email);
           
            if (account != null)
                return Helper.GetResponse<SignUpResponse>(statusCode: StatusCodeValue.Fail, errorCode: ConstantValue.Err1006, message: ConstantValue.Err1006Message);

            // insert new account user to database
            string newAccountId = ObjectId.GenerateNewId().ToString();
            string newUserId = ObjectId.GenerateNewId().ToString();
            (string salt, string hashed) paswordHashed = Helper.HashPassword(param.Password);
            UserEntity newUser = new() { Id = newUserId,Email = param.Email, FullName = param.FullName};
            AccountEntity newAccount = new() { Id = newAccountId, Email = param.Email,UserId = newUserId,Password  = string.Join(ConstantValue.PasswordHashDelimiter, paswordHashed.salt,paswordHashed.hashed)};
            Thread thread1 = new(async () => await _database.AccountColection().CreateNewAccount(newAccount));
            Thread thread2 = new(async () => await _database.UserColection().CreateNewUser(newUser));
            thread1.Start();
            thread2.Start();


            // gen authen token
            string token = Helper.CreateJWTToken(newUser);
            SignUpResponse res = new() { User = _mapper.Map<UserEntity, UserDTO>(newUser), Token = token };
            return Helper.GetResponse(data: res);

        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
            return Helper.GetResponse<SignUpResponse>(statusCode: StatusCodeValue.Fail, errorCode: ConstantValue.Err0001, message: $"Error: {e.Message}");
        }

    }
}