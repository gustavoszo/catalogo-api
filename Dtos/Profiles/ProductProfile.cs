using AutoMapper;
using CatalogoApi.Models;

namespace CatalogoApi.Dtos.Profiles
{
    public class ProductProfile : Profile
    {

        public ProductProfile() 
        {
            CreateMap<Product, ProductRequestDto>().ReverseMap();
            CreateMap<Product, ProductResponseDto>()
                // .ForMember(productDto => productDto.Category, opt => opt.MapFrom(product => product.Category))
                .ReverseMap();
            CreateMap<Product, ProductUpdateDto>().ReverseMap();
        }

    }
}
