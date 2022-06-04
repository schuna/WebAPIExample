using AutoMapper;
using WebApi.DataAccess.DTOs.User;
using WebApi.DataAccess.Models;

namespace WebApi.DataAccess.Helpers;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<UserCreateDto, User>();
        CreateMap<UserUpdateDto, User>().ForAllMembers(x
            => x.Condition(
                (src, dest, prop) =>
                {
                    if (prop == null) return false;
                    if (prop is string && string.IsNullOrEmpty((string)prop)) return false;
                    return true;
                }
            ));
    }
}