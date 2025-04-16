using GeneratorStores.DataAccess.Entities;
using System.Net.Http.Json;
using GeneratorStores.DataAccess.Dtos;

namespace GeneratorStores.DataAccess.Services;

public class OrderService : IOrderService
{
    private readonly HttpClient _httpClient;

    public OrderService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<Order>> GetAllOrdersAsync()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<Order>>("api/Orders");
    }

    public async Task<Order> GetOrderByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<Order>($"api/Orders/{id}");
    }

    public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/orders/user/{userId}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<IEnumerable<Order>>() ?? new List<Order>();
            }
            // If the status code is not 200, return an empty list
            return new List<Order>();
        }
        catch (Exception ex)
        {
            // Log the exception (optional)
            Console.WriteLine($"Error fetching orders for user {userId}: {ex.Message}");
            return new List<Order>();
        }
    }


    public async Task CreateOrderAsync(Order order)
    {
        var response = await _httpClient.PostAsJsonAsync("api/Orders", order);
        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateOrderAsync(Order order)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/Orders/{order.Id}", order);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteOrderAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/Orders/{id}");
        response.EnsureSuccessStatusCode();
    }

    public async Task PlaceOrderAsync(IEnumerable<Product> products)
{
    var productIds = products.Select(p => p.Id).ToList();

    var userResponse = await _httpClient.GetAsync("api/Users/current"); // Endpoint to get the current user
    if (!userResponse.IsSuccessStatusCode)
    {
        throw new Exception("Failed to fetch the current user.");
    }

    var currentUser = await userResponse.Content.ReadFromJsonAsync<ApplicationUser>();
    if (currentUser == null)
    {
        throw new Exception("Current user could not be determined.");
    }

    var createOrderDto = new CreateOrderDto
    {
        CustomerId = currentUser.Id,
        ProductIds = productIds
    };

    var response = await _httpClient.PostAsJsonAsync("api/Orders", createOrderDto);
    response.EnsureSuccessStatusCode();
}

}
