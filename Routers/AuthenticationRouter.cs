using MonTraApi.Domains.DTOs;
using MonTraApi.Domains.Services;

namespace MonTraApi.Routers;

public static class AuthenticationRouter
{
    public static void AuthenticationMap(this IEndpointRouteBuilder builder)
    {
        string groupName = "auth";
        string tag = "Authentication";

        builder.MapPost($"/{groupName}/login", async (IAuthenticationService authenticationService, LoginRequest param) => await authenticationService.Login(param: param)).WithTags(tag);
    }

}

