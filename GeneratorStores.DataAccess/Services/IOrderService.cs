using GeneratorStores.DataAccess.Dtos;
using GeneratorStores.DataAccess.Entities;

namespace GeneratorStores.DataAccess.Services;

public interface IOrderService
{
    Task<IEnumerable<Order>> GetAllOrdersAsync();
    Task<Order> GetOrderByIdAsync(int id);
    Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId);
    Task CreateOrderAsync(Order order);
    Task UpdateOrderAsync(Order order);
    Task DeleteOrderAsync(int id);
    Task CreateOrderFromDtoAsync(CreateOrderDto createOrderDto);


}



