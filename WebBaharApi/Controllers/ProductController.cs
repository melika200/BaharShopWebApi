using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bahar.Application.InterfaceRepository;
using Bahar.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using Bahar.Application.Dto.Product;

namespace WebBaharApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;
        private readonly ICategoryRepository _categoryRepository;

        public ProductController(ProductService productService, ICategoryRepository categoryRepository)
        {
            _productService = productService;
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ProductDto>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _productService.GetAllProducts();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(products);
        }

        [HttpGet("{productId}")]
        [ProducesResponseType(200, Type = typeof(ProductDto))]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetProduct(long productId)
        {
            var product = await _productService.GetProductById(productId);
            if (product == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(product);
        }

        [HttpGet("pagination")]
        [ProducesResponseType(200, Type = typeof(PagedProductDto))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetProductsPagination(int page = 1, int pageSize = 10, string search = null, string sortOrder = "newest")
        {
            var pagedResult = await _productService.GetProductPagination(page, pageSize, search, sortOrder);
            return Ok(pagedResult);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        public async Task<IActionResult> CreateProduct([FromBody] ProductCreateDto productCreateDto)
        {
            if (productCreateDto == null)
                return BadRequest("Product data is null.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            bool categoryExists = await _categoryRepository.CategoryExists(productCreateDto.CategoryId);
            if (!categoryExists)
            {
                ModelState.AddModelError("CategoryId", "Category does not exist.");
                return UnprocessableEntity(ModelState);
            }

            try
            {
                var createdProductDto = await _productService.CreateProduct(productCreateDto);
                return CreatedAtAction(nameof(GetProduct), new { productId = createdProductDto.Id }, createdProductDto);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Something went wrong creating the product: {ex}");
                return StatusCode(500, ModelState);
            }
        }

        [HttpPut("{productId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateProduct(long productId, [FromBody] UpdateProductDto updatedProductDto)
        {
            if (updatedProductDto == null)
                return BadRequest("Product data is null.");

           

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            bool categoryExists = await _categoryRepository.CategoryExists(updatedProductDto.CategoryId);
            if (!categoryExists)
            {
                ModelState.AddModelError("CategoryId", "Category does not exist.");
                return BadRequest(ModelState);
            }

            try
            {
                await _productService.UpdateProduct(productId, updatedProductDto);
                return NoContent();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Product not found"))
                    return NotFound(ex.Message);

                ModelState.AddModelError("", $"Something went wrong updating the product: {ex}");
                return StatusCode(500, ModelState);
            }
        }

        [HttpDelete("{productId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteProduct(long productId)
        {
            try
            {
                await _productService.DeleteProduct(productId);
                return NoContent();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Product not found"))
                    return NotFound(ex.Message);

                ModelState.AddModelError("", $"Something went wrong deleting the product: {ex}");
                return StatusCode(500, ModelState);
            }
        }

        [HttpGet("{productId}/stock")]
        [ProducesResponseType(200, Type = typeof(int))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetProductStock(long productId)
        {
            var product = await _productService.GetProductById(productId);
            if (product == null)
                return NotFound("Product not found.");

            return Ok(product.Stock);
        }

        [HttpPost("{productId}/rate")]
        [ProducesResponseType(200, Type = typeof(object))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> RateProduct(long productId, [FromBody] int rating)
        {
            if (rating < 1 || rating > 5)
                return BadRequest("Rating must be between 1 and 5.");

            try
            {
                var result = await _productService.RateProduct(productId, rating);
                if (!result)
                    return NotFound("Product not found.");

                var updatedProduct = await _productService.GetProductById(productId);
                return Ok(new { success = true, message = "Rating submitted successfully.", newAverageRating = updatedProduct.AverageRating });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred while rating the product: {ex}");
                return StatusCode(500, ModelState);
            }
        }
    }
}
