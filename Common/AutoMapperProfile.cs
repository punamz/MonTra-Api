using AutoMapper;
using MonTraApi.Domains.DTOs;
using MonTraApi.Domains.Entities;

namespace MonTraApi.Common;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<UserEntity, UserDTO>();
        CreateMap<CategoryEntity, CategoryDTO>();
    }

}
