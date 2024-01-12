using AutoMapper;
using MongoDB.Driver;
using MonTraApi.Common;
using MonTraApi.Domains.DTOs;
using MonTraApi.Domains.Entities;
using MonTraApi.Domains.Services;
using System.Diagnostics;

namespace MonTraApi.Infrastructures.Repositories;

public class AuthenticationRepository : IAuthenticationService
{

    private readonly IMongoCollection<UserEntity> UserColection;

    private readonly IMongoCollection<AccountEntity> AccountColection;


    private readonly IMapper Mapper;

    public AuthenticationRepository(IMongoDatabase database, IMapper mapper)
    {
        UserColection = database.GetCollection<UserEntity>("User");
        AccountColection = database.GetCollection<AccountEntity>("Account");
        Mapper = mapper;
    }

    public async Task<ResultDTO<UserDTO>> Login(LoginParam param)
    {
        try
        {
            AccountEntity account = await AccountColection.Find(account => account.Email == param.Email && account.Password == param.Password).FirstOrDefaultAsync();

            if (account == null)
                return new ResultDTO<UserDTO>()
                {
                    Code = CodeValue.NoData,
                    ErrorCode = ConstantValue.err1001,
                    Message = "Email or password incorrect",
                };

            UserEntity user = await UserColection.Find(user => user.Id == account.UserId).FirstOrDefaultAsync();
            if (user == null)
                return new ResultDTO<UserDTO>()
                {
                    Code = CodeValue.NoData,
                    ErrorCode = ConstantValue.err1002,
                    Message = "User not found",
                };

            return new ResultDTO<UserDTO>()
            {
                Code = CodeValue.Success,
                Data = Mapper.Map<UserEntity, UserDTO>(user)
            };

        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
            return new ResultDTO<UserDTO>()
            {
                Code = CodeValue.Fail,
                ErrorCode = ConstantValue.err0001,
                Message = $"Lỗi: {e.Message}",
            };
        }

    }
}
