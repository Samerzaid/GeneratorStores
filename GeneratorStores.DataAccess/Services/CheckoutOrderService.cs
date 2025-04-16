using GeneratorStores.DataAccess.Entities;

namespace GeneratorStores.DataAccess.Services;

public class CheckoutOrderService : ICheckoutOrderService
{
    private readonly List<Order> _orders = new();

    public Task PlaceOrder(string userId, IEnumerable<Product> cartItems)
    {
        var order = new Order
        {
            UserId = userId,
            TotalPrice = cartItems.Sum(item => item.Price),
            ProductsInOrder = cartItems.Select(item => new ProductOrder
            {
                ProductId = item.Id,
                ProductName = item.ProductName,
                UnitPrice = item.Price,
                Amount = 1 // Assuming a default quantity of 1
            }).ToList()
        };

        _orders.Add(order);

        // Replace this with database logic if you're using Entity Framework
        return Task.CompletedTask;
    }

    public Task<List<Order>> GetUserOrders(string userId)
    {
        var userOrders = _orders.Where(o => o.UserId == userId).ToList();
        return Task.FromResult(userOrders);
    }
}
