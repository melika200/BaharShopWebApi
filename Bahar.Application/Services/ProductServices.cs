using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Bahar.Application.Dto.Product;
using Bahar.Application.InterfaceRepository;
using Bahar.Domain;

namespace Bahar.Application.Services
{
    public class ProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProducts()
        {
            var products = await _productRepository.GetAllWithCategory();
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<ProductDto> GetProductById(long id)
        {
            var product = await _productRepository.GetById(id);
            return _mapper.Map<ProductDto>(product);
        }

        public async Task<ProductDto> CreateProduct(ProductCreateDto productCreateDto)
        {
            var product = _mapper.Map<Product>(productCreateDto);

            product.InsertTime = DateTime.Now;
            product.TotalRatings = 0;
            product.AverageRating = 0;

            await _productRepository.Add(product);

            return _mapper.Map<ProductDto>(product);
        }



        public async Task UpdateProduct(long id, ProductDto productDto)
        {
            var product = await _productRepository.GetById(id);
            if (product == null)
                throw new Exception("Product not found");

            _mapper.Map(productDto, product);
            await _productRepository.Update(product);
        }

        public async Task DeleteProduct(long id)
        {
            await _productRepository.Delete(id);
        }
        public async Task<bool> RateProduct(long productId, int rating)
        {
            if (rating < 1 || rating > 5)
                throw new ArgumentException("Rating must be between 1 and 5.");

            var product = await _productRepository.GetById(productId);
            if (product == null)
                return false;

            product.TotalRatings++;
            product.AverageRating = ((product.AverageRating * (product.TotalRatings - 1)) + rating) / product.TotalRatings;

            await _productRepository.Update(product);

            return true;
        }
        public async Task<PagedProductDto> GetProductPagination(int page, int pageSize, string search, string sortOrder)
        {
            var products = await _productRepository.GetAll();

            if (!string.IsNullOrEmpty(search))
            {
                products = products.Where(p => p.Name.Contains(search) || p.Description.Contains(search)).ToList();
            }

            switch (sortOrder)
            {
                case "highest":
                    products = products.OrderByDescending(p => p.Price).ToList();
                    break;
                case "cheapest":
                    products = products.OrderBy(p => p.Price).ToList();
                    break;
                default:
                    products = products.OrderByDescending(p => p.InsertTime).ToList();
                    break;
            }

            int totalCount = products.Count();
            int totalPage = (int)Math.Ceiling((double)totalCount / pageSize);

            products = products.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var productDtos = products.Select(p => _mapper.Map<ProductDto>(p)).ToList();

            return new PagedProductDto
            {
                Items = productDtos,
                Page = page,
                TotalPage = totalPage
            };
        }

    }

}
