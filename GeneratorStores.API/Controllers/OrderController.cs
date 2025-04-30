using GeneratorStores.DataAccess.Dtos;
using GeneratorStores.DataAccess.Entities;
using GeneratorStores.DataAccess.Interfaces;
using GeneratorStores.DataAccess.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using ApplicationUser = GeneratorStores.DataAccess.Entities.ApplicationUser;

namespace GeneratorStores.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IEmailService _emailService;

    public OrdersController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IEmailService emailService)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _emailService = emailService;
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
        if (order == null) return NotFound();
        return Ok(order);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetOrdersByUserId(string userId)
    {
        var orders = await _unitOfWork.Orders.GetOrdersByUserIdAsync(userId);
        if (!orders.Any()) return NotFound($"No orders found for User ID {userId}");
        return Ok(orders);
    }

    // ✅ Create full order after PayPal checkout
    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] Order order)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var user = await _userManager.FindByIdAsync(order.UserId);
        if (user == null) return BadRequest("User not found.");

        order.User = user;
        order.OrderDate = DateTime.UtcNow;

        await _unitOfWork.Orders.AddAsync(order);
        await _unitOfWork.CompleteAsync();

        return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
    }

    // ✅ Keep support for CreateOrderDto if needed
    [HttpPost("dto")]
    public async Task<IActionResult> CreateOrderFromDto([FromBody] CreateOrderDto createOrderDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var user = await _userManager.FindByIdAsync(createOrderDto.CustomerId);
        if (user == null) return BadRequest($"User with ID {createOrderDto.CustomerId} not found.");

        var products = createOrderDto.ProductIds.Select(id => _unitOfWork.Products.GetByIdAsync(id).Result).ToList();
        var totalPrice = products.Sum(p => p.Price);

        var order = new Order
        {
            UserId = user.Id,
            User = user,
            TotalPrice = totalPrice,
            OrderDate = DateTime.UtcNow,
            Status = "Approved", // ✅ Force the Status to Approved here
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

        var emailBody = $"""
<table style="width: 100%; max-width: 700px; margin: auto; font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; border: 1px solid #e0e0e0; padding: 20px; background-color: #f9f9f9;">
    <tr>
        <td style="text-align: center;">
            <h2 style="color: #4CAF50;">Thank You for Your Order, {user.FullName}!</h2>
            <p style="font-size: 16px; color: #555;">We're getting your order ready to ship. We will notify you when it has been sent.</p>
        </td>
    </tr>

    <tr>
        <td style="padding: 20px 0;">
            <h3 style="border-bottom: 1px solid #ddd; padding-bottom: 8px; color: #333;">Order Summary</h3>

            <table style="width: 100%; border-collapse: collapse; margin-top: 10px;">
                <thead style="background-color: #eee;">
                    <tr>
                        <th style="text-align: left; padding: 8px;">Product</th>
                        <th style="text-align: center; padding: 8px;">Quantity</th>
                        <th style="text-align: right; padding: 8px;">Price</th>
                    </tr>
                </thead>
                <tbody>
                    {string.Join("", order.ProductsInOrder.Select(p => $"""
                        <tr style="border-top: 1px solid #ddd;">
                            <td style="padding: 8px;">{p.ProductName}</td>
                            <td style="padding: 8px; text-align: center;">{p.Amount}</td>
                            <td style="padding: 8px; text-align: right;">${p.UnitPrice.ToString("0.00")}</td>
                        </tr>
                    """))}
                </tbody>
            </table>

            <p style="margin-top: 20px; font-size: 16px;">
                <strong>Order ID:</strong> {order.Id}<br/>
                <strong>Order Date:</strong> {order.OrderDate:MMMM dd, yyyy - HH:mm}<br/>
                <strong>Status:</strong> {order.Status}<br/>
                <strong>Total:</strong> <span style="color: #4CAF50;">${order.TotalPrice.ToString("0.00")}</span>
            </p>
        </td>
    </tr>

    <tr>
        <td style="text-align: center; padding-top: 30px;">
            <p style="font-size: 15px; color: #777;">If you have any questions, reply to this email.</p>
            <p style="font-size: 15px; color: #777;">Thank you for shopping with <strong>Zalando-Phones</strong>!</p>
        </td>
    </tr>
</table>
""";



        await _emailService.SendEmailAsync(
            to: user.Email,
            subject: "🛒 Order Confirmation - Zalando-Phones",
            body: emailBody
        );



        return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
    }



    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateOrder(int id, [FromBody] Order order)
    {
        if (id != order.Id) return BadRequest("Order ID mismatch");

        var existingOrder = await _unitOfWork.Orders.GetByIdAsync(id);
        if (existingOrder == null) return NotFound();

        await _unitOfWork.Orders.UpdateAsync(order);
        await _unitOfWork.CompleteAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        var existingOrder = await _unitOfWork.Orders.GetByIdAsync(id);
        if (existingOrder == null) return NotFound();

        await _unitOfWork.Orders.DeleteAsync(id);
        await _unitOfWork.CompleteAsync();
        return NoContent();
    }

    [HttpPost("create-full")]
    public async Task<IActionResult> CreateFullOrder([FromBody] Order order)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await _userManager.FindByIdAsync(order.UserId);
        if (user == null)
            return BadRequest("User not found.");

        order.User = user;
        order.OrderDate = DateTime.UtcNow;

        // Optional: Set default status or TransId if not already set
        order.Status ??= "Pending";

        await _unitOfWork.Orders.AddAsync(order);
        await _unitOfWork.CompleteAsync();

        return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
    }


    [HttpGet("stats/total-orders")]
    public async Task<IActionResult> GetTotalOrders()
    {
        var orders = await _unitOfWork.Orders.GetAllAsync();
        var totalOrders = orders.Count();
        return Ok(totalOrders);
    }

    [HttpGet("stats/total-sales")]
    public async Task<IActionResult> GetTotalSales()
    {
        var orders = await _unitOfWork.Orders.GetAllAsync();
        var totalSales = orders.Sum(o => o.TotalPrice);
        return Ok(totalSales);
    }

    [HttpGet("stats/sales-over-time")]
    public async Task<IActionResult> GetSalesOverTime()
    {
        var orders = await _unitOfWork.Orders.GetAllAsync();

        var salesOverTime = orders
            .GroupBy(o => o.OrderDate.Date)
            .Select(g => new
            {
                Date = g.Key,
                TotalSales = g.Sum(o => o.TotalPrice)
            })
            .OrderBy(x => x.Date)
            .ToList();

        return Ok(salesOverTime);
    }


    [HttpGet("stats/today-sales")]
    public async Task<IActionResult> GetTodaySales()
    {
        var orders = await _unitOfWork.Orders.GetAllAsync();
        var today = DateTime.UtcNow.Date;
        var todaySales = orders
            .Where(o => o.OrderDate.Date == today)
            .Sum(o => o.TotalPrice);
        return Ok(todaySales);
    }

    [HttpGet("stats/month-sales")]
    public async Task<IActionResult> GetMonthSales()
    {
        var orders = await _unitOfWork.Orders.GetAllAsync();
        var now = DateTime.UtcNow;
        var monthSales = orders
            .Where(o => o.OrderDate.Month == now.Month && o.OrderDate.Year == now.Year)
            .Sum(o => o.TotalPrice);
        return Ok(monthSales);
    }


}




