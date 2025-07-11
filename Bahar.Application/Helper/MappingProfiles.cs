using AutoMapper;
using Bahar.Application.Dto;
using Bahar.Application.Dto.Cart;
using Bahar.Application.Dto.Product;
using Bahar.Domain;

namespace WebBaharApi.Helper
{
    public class MappingProfiles:Profile
    {
        public MappingProfiles()
        {
            CreateMap<Category, CategoryDto>();
            CreateMap<Product, ProductDto>();
            CreateMap<CartItem, CartItemDto>();
         
          



            CreateMap<CategoryDto, Category>();
            CreateMap<ProductDto, Product>();
            CreateMap<ProductCreateDto, Product>();
            CreateMap<CartItemDto, CartItem>();
        }
    }
}
