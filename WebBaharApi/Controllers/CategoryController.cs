using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Bahar.Application.Dto.Category;
using Bahar.Application.Dto.Product;
using Bahar.Application.InterfaceRepository;
using Bahar.Domain;
using Microsoft.AspNetCore.Mvc;

namespace WebBaharApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CategoryDto>))]
        public async Task<IActionResult> GetCategories()
        {
            var categoriesFromRepo = await _categoryRepository.GetAll();
          
            var categories = _mapper.Map<List<CategoryDto>>(categoriesFromRepo);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(categories);
        }

        [HttpGet("{categoryId}")]
        [ProducesResponseType(200, Type = typeof(CategoryDto))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetCategory(int categoryId)
        {
            if (!await _categoryRepository.CategoryExists(categoryId))
                return NotFound();

            var categoryFromRepo = await _categoryRepository.GetById(categoryId);
            var category = _mapper.Map<CategoryDto>(categoryFromRepo);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(category);
        }

        [HttpGet("product/{categoryId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ProductDto>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetProductByCategoryId(int categoryId)
        {
            var productsFromRepo = await _categoryRepository.GetProductsByCategory(categoryId);
            var products = _mapper.Map<List<ProductDto>>(productsFromRepo);

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(products);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryCreateDto categoryCreate)
        {
            if (categoryCreate == null)
                return BadRequest("Category data is null.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
        
            var categories = await _categoryRepository.GetAll(c => c.Name.Trim().ToUpper() == categoryCreate.Name.Trim().ToUpper());
          
            if (categories.Any())
            {
                ModelState.AddModelError("", "Category already exists");
                return UnprocessableEntity(ModelState);
            }

            var categoryMap = _mapper.Map<Category>(categoryCreate);

            await _categoryRepository.Add(categoryMap);
           
            return CreatedAtAction(nameof(GetCategory), new { categoryId = categoryMap.Id }, categoryMap);
        }

        [HttpPut("{categoryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateCategory(int categoryId, [FromBody] CategoryUpdateDto updatedCategory)
        {
            if (updatedCategory == null)
                return BadRequest("Category data is null.");

            if (!await _categoryRepository.CategoryExists(categoryId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            
            var categoryFromDb = await _categoryRepository.GetById(categoryId);
            if (categoryFromDb == null)
                return NotFound();

            _mapper.Map(updatedCategory, categoryFromDb);
           

            try
            {
                await _categoryRepository.Update(categoryFromDb);
            }
            catch
            {
                ModelState.AddModelError("", "Something went wrong updating category");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }


        [HttpDelete("{categoryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteCategory(long categoryId)
        {
            if (!await _categoryRepository.CategoryExists(categoryId))
                return NotFound();

            var categoryToDelete = await _categoryRepository.GetById(categoryId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _categoryRepository.Delete(categoryId);
            }
            catch
            {
                ModelState.AddModelError("", "Something went wrong deleting category");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
