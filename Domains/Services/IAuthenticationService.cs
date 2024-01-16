using MonTraApi.Domains.DTOs;

namespace MonTraApi.Domains.Services;

public interface IAuthenticationService
{
    Task<ResultDTO<LoginResponse>> Login(LoginRequest param);
    Task<ResultDTO<SignUpResponse>> Registration(SignUpRequest param);

}
