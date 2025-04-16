using GeneratorStores.DataAccess.Entities;

namespace GeneratorStores.DataAccess.Interfaces;

public interface IProductOrderRepository : IRepository<ProductOrder, int>
{
    Task<IEnumerable<ProductOrder>> GetProductOrdersByOrderIdAsync(int orderId);
}
