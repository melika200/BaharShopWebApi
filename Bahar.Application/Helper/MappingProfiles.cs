using AutoMapper;
using Bahar.Application.Dto;
using Bahar.Domain;

namespace WebBaharApi.Helper
{
    public class MappingProfiles:Profile
    {
        public MappingProfiles()
        {
            CreateMap<Category, CategoryDto>();
            CreateMap<Product, ProductDto>();


            CreateMap<CategoryDto, Category>();
            CreateMap<ProductDto, Product>();
        }
    }
}
