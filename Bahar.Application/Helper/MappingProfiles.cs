using AutoMapper;
using Bahar.Application.Dto.Cart;
using Bahar.Application.Dto.Category;
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
            CreateMap<CategoryCreateDto, Category>();
            CreateMap<CategoryUpdateDto, Category>();
            CreateMap<ProductCreateDto, Product>();
            CreateMap<UpdateProductDto, Product>();
            CreateMap<CartItemDto, CartItem>();
        }
    }
}
