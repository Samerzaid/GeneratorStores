using GeneratorStores.DataAccess.Dtos;
using GeneratorStores.DataAccess.Entities;
using GeneratorStores.DataAccess.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using ApplicationUser = GeneratorStores.DataAccess.Entities.ApplicationUser;

namespace GeneratorStores.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<ApplicationUser> _userManager;

    public OrdersController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
    }


    [HttpGet]
    public async Task<IActionResult> GetAllOrders()
    {
        var orders = await _unitOfWork.Orders.GetAllAsync();
        return Ok(orders);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderById(int id)
    {
        var order = await _unitOfWork.Orders.GetByIdAsync(id);
        if (order == null)
        {
            return NotFound();
        }
        return Ok(order);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetOrdersByUserId(string userId)
    {
        var orders = await _unitOfWork.Orders.GetOrdersByUserIdAsync(userId);
        if (!orders.Any())
        {
            return NotFound($"No orders found for User ID {userId}");
        }
        return Ok(orders);
    }


    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto createOrderDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Fetch the user using UserManager
        var user = await _userManager.FindByIdAsync(createOrderDto.CustomerId);
        if (user == null)
        {
            return BadRequest($"User with ID {createOrderDto.CustomerId} not found.");
        }

        // Fetch the products
        var products = createOrderDto.ProductIds.Select(id => _unitOfWork.Products.GetByIdAsync(id).Result).ToList();
        var totalPrice = products.Sum(p => p.Price);

        // Create the order
        var order = new Order
        {
            UserId = user.Id,
            User = user,
            TotalPrice = totalPrice,
            OrderDate = DateTime.UtcNow,
            ProductsInOrder = products.Select(p => new ProductOrder
            {
                ProductId = p.Id,
                ProductName = p.ProductName,
                UnitPrice = p.Price,
                Amount = 1
            }).ToList()
        };

        await _unitOfWork.Orders.AddAsync(order);
        await _unitOfWork.CompleteAsync();

        return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
    }




    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateOrder(int id, [FromBody] Order order)
    {
        if (id != order.Id)
        {
            return BadRequest("Order ID mismatch");
        }

        var existingOrder = await _unitOfWork.Orders.GetByIdAsync(id);
        if (existingOrder == null)
        {
            return NotFound();
        }

        await _unitOfWork.Orders.UpdateAsync(order);
        await _unitOfWork.CompleteAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        var existingOrder = await _unitOfWork.Orders.GetByIdAsync(id);
        if (existingOrder == null)
        {
            return NotFound();
        }

        await _unitOfWork.Orders.DeleteAsync(id);
        await _unitOfWork.CompleteAsync();
        return NoContent();
    }
}

