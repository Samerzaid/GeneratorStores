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

            return new List<Order>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching orders for user {userId}: {ex.Message}");
            return new List<Order>();
        }
    }

    // Used after PayPal checkout – sends full Order with TransId, Status, etc.
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




    public async Task CreateOrderFromDtoAsync(CreateOrderDto createOrderDto)
    {
        var response = await _httpClient.PostAsJsonAsync("api/orders/dto", createOrderDto);
        response.EnsureSuccessStatusCode();
    }


    public async Task<int> GetTotalOrdersAsync()
    {
        return await _httpClient.GetFromJsonAsync<int>("api/orders/stats/total-orders");
    }

    public async Task<decimal> GetTotalSalesAsync()
    {
        return await _httpClient.GetFromJsonAsync<decimal>("api/orders/stats/total-sales");
    }

    public async Task<IEnumerable<SalesOverTimeDto>> GetSalesOverTimeAsync()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<SalesOverTimeDto>>("api/orders/stats/sales-over-time");
    }


    public async Task<decimal> GetTodaySalesAsync()
    {
        return await _httpClient.GetFromJsonAsync<decimal>("api/orders/stats/today-sales");
    }

    public async Task<decimal> GetMonthSalesAsync()
    {
        return await _httpClient.GetFromJsonAsync<decimal>("api/orders/stats/month-sales");
    }


}




