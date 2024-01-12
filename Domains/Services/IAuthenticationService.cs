using MonTraApi.Domains.DTOs;

namespace MonTraApi.Domains.Services;

public interface IAuthenticationService
{
    Task<ResultDTO<UserDTO>> Login(LoginParam param);

}
