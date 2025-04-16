using GeneratorStores.DataAccess.Entities;
using GeneratorStores.DataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GeneratorStores.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public ProductsController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllProducts()
    {
        var products = await _unitOfWork.Products.GetAllAsync();
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(int id)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] Product product)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await _unitOfWork.Products.AddAsync(product);
        await _unitOfWork.CompleteAsync();
        return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product product)
    {
        if (id != product.Id)
        {
            return BadRequest("Product ID mismatch");
        }

        var existingProduct = await _unitOfWork.Products.GetByIdAsync(id);
        if (existingProduct == null)
        {
            return NotFound("Product not found.");
        }

        try
        {
            await _unitOfWork.Products.UpdateAsync(product);
            await _unitOfWork.CompleteAsync();
            return NoContent();
        }
        catch (Exception ex)
        {
            // Log the exception for debugging
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var existingProduct = await _unitOfWork.Products.GetByIdAsync(id);
        if (existingProduct == null)
        {
            return NotFound();
        }

        await _unitOfWork.Products.DeleteAsync(id);
        await _unitOfWork.CompleteAsync();
        return NoContent();
    }
}


