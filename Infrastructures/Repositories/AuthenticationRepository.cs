using AutoMapper;
using MongoDB.Driver;
using MonTraApi.Common;
using MonTraApi.Domains.DTOs;
using MonTraApi.Domains.Entities;
using MonTraApi.Domains.Services;
using MonTraApi.Infrastructures.Queries;
using System.Diagnostics;
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

    public async Task<ResultDTO<UserDTO>> Login(LoginParam param)
    {
        try
        {
            // check valid param
            if (param.Email == null || param.Password == null)
                return Helper.GetResponse<UserDTO>(statusCode: StatusCodeValue.Fail, errorCode: ConstantValue.Err0002, message: ConstantValue.Err0002Message);

            AccountEntity? account = await _database.AccountColection().GetAccountByEmailPassword(param.Email, param.Password);

            if (account == null)
                return Helper.GetResponse<UserDTO>(statusCode: StatusCodeValue.NoData, errorCode: ConstantValue.Err1001, message: ConstantValue.Err1001Message);

            UserEntity? user = await _database.UserColection().GetUserById(account.UserId);
            if (user == null)
                return Helper.GetResponse<UserDTO>(statusCode: StatusCodeValue.NoData, errorCode: ConstantValue.Err1002, message: ConstantValue.Err1002Message);

            return Helper.GetResponse(data: _mapper.Map<UserEntity, UserDTO>(user));
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
            return Helper.GetResponse<UserDTO>(statusCode: StatusCodeValue.Fail, errorCode: ConstantValue.Err0001, message: $"Error: {e.Message}");
        }

    }
}
