using GeneratorStores.DataAccess.Entities;
using System.Net.Http.Json;

namespace GeneratorStores.DataAccess.Services;

public class ProductService : IProductService
{
    private readonly HttpClient _httpClient;

    public ProductService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<Product>>("api/Products");
    }

    public async Task<Product> GetProductByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<Product>($"api/Products/{id}");
    }

    public async Task AddProductAsync(Product product)
    {
        var response = await _httpClient.PostAsJsonAsync("api/Products", product);
        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateProductAsync(Product product)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/Products/{product.Id}", product);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteProductAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/Products/{id}");
        response.EnsureSuccessStatusCode();
    }
}


