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

    public async Task<IEnumerable<Product>> GetNewArrivalsAsync(int count = 6)
    {
        var all = await GetAllProductsAsync();
        return all.OrderByDescending(p => p.DateAdded).Take(count);
    }

    public async Task<IEnumerable<Product>> GetOldArrivalsAsync(int count = 6)
    {
        var all = await GetAllProductsAsync();
        return all
            .OrderBy(p => p.DateAdded) // ✅ shows oldest first
            .Take(count);
    }

    public async Task<IEnumerable<Product>> GetRelatedProductsAsync(int currentProductId, int count = 4)
    {
        var allProducts = await GetAllProductsAsync();
        var allCategories = await _httpClient.GetFromJsonAsync<IEnumerable<CategoryProduct>>("api/CategoryProducts");

        // Get the categoryIds for the current product
        var currentProductCategoryIds = allCategories
            .Where(cp => cp.ProductId == currentProductId)
            .Select(cp => cp.CategoryId)
            .Distinct()
            .ToList();

        // Get related products in the same categories, excluding the current product
        var relatedProductIds = allCategories
            .Where(cp => currentProductCategoryIds.Contains(cp.CategoryId) && cp.ProductId != currentProductId)
            .Select(cp => cp.ProductId)
            .Distinct()
            .ToList();

        var relatedProducts = allProducts
            .Where(p => relatedProductIds.Contains(p.Id))
            .Take(count)
            .ToList();

        return relatedProducts;
    }


}



