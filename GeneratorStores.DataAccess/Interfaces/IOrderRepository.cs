using GeneratorStores.DataAccess.Entities;
using GeneratorStores.DataAccess.Interfaces;

namespace GeneratorStore.DataAccess.Interfaces;

public interface IOrderRepository : IRepository<Order, int>
{
    Task<IEnumerable<Order>> GetOrdersWithProductsAsync();

    Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId);
}

