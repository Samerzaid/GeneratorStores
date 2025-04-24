using GeneratorStores.DataAccess.Entities;

namespace GeneratorStores.DataAccess.Services;

public interface ICheckoutOrderService
{
    Task PlaceOrder(string userId, IEnumerable<Product> cartItems);
    Task<List<Order>> GetUserOrders(string userId);

}



