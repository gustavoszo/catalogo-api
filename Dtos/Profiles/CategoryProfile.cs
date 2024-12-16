using AutoMapper;
using CatalogoApi.Models;

namespace CatalogoApi.Dtos.Profiles
{
    public class CategoryProfile : Profile
    {

        public CategoryProfile() 
        {
            CreateMap<CategoryRequestDto, Category>().ReverseMap();
            CreateMap<CategoryResponseDto, Category>().ReverseMap();
        }

    }
}
