using GeneratorStores.DataAccess.Dtos;
using GeneratorStores.DataAccess.Entities;
using GeneratorStores.DataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GeneratorStores.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/category
        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync();
            return Ok(categories);
        }

        // GET: api/category/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound($"Category with ID {id} not found.");
            }
            return Ok(category);
        }

        // GET: api/category/with-products
        [HttpGet("with-products")]
        public async Task<IActionResult> GetCategoriesWithProducts()
        {
            var categories = await _unitOfWork.Categories.GetCategoriesWithProductsAsync();
            return Ok(categories);
        }

        // POST: api/category
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _unitOfWork.Categories.AddAsync(category);
            await _unitOfWork.CompleteAsync();

            return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, category);
        }

        // PUT: api/category/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] Category category)
        {
            if (id != category.Id)
            {
                return BadRequest("Category ID mismatch.");
            }

            var existingCategory = await _unitOfWork.Categories.GetByIdAsync(id);
            if (existingCategory == null)
            {
                return NotFound($"Category with ID {id} not found.");
            }

            existingCategory.Name = category.Name;
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }

        // DELETE: api/category/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound($"Category with ID {id} not found.");
            }

            await _unitOfWork.Categories.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }

        // POST: api/category/add-products
        [HttpPost("add-products")]
        public async Task<IActionResult> AddProductsToCategory([FromBody] CategoryProductDto categoryProductDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Validate if the category exists
            var category = await _unitOfWork.Categories.GetByIdAsync(categoryProductDto.CategoryId);
            if (category == null)
            {
                return NotFound($"Category with ID {categoryProductDto.CategoryId} not found.");
            }

            // Add each product to the category
            foreach (var productId in categoryProductDto.ProductIds)
            {
                var product = await _unitOfWork.Products.GetByIdAsync(productId);
                if (product == null)
                {
                    return NotFound($"Product with ID {productId} not found.");
                }

                var categoryProduct = new CategoryProduct
                {
                    CategoryId = categoryProductDto.CategoryId,
                    ProductId = productId
                };

                await _unitOfWork.CategoryProducts.AddAsync(categoryProduct);
            }

            // Save changes
            await _unitOfWork.CompleteAsync();
            return Ok("Products successfully added to the category.");
        }
    }

}

