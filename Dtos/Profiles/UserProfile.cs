using AutoMapper;
using CatalogoApi.Models;

namespace CatalogoApi.Dtos.Profiles
{
    public class UserProfile : Profile
    {

        public UserProfile()
        {
            CreateMap<LoginDto, User>();
            CreateMap<UserRegisterDto, User>();
        }

    }
}
