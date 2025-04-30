using GeneratorStores.DataAccess.Dtos;
using GeneratorStores.DataAccess.Entities;
using System.Net.Http.Json;

namespace GeneratorStores.DataAccess.Services;

public class CategoryService : ICategoryService
{
    private readonly HttpClient _httpClient;

    public CategoryService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<Category>>("api/category");
    }

    public async Task<Category> GetCategoryByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<Category>($"api/category/{id}");
    }

    public async Task<IEnumerable<CategoryWithProductsDto>> GetCategoriesWithProductsAsync()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<CategoryWithProductsDto>>("api/category/with-products");
    }



    public async Task AddCategoryAsync(Category category)
    {
        var response = await _httpClient.PostAsJsonAsync("api/category", category);
        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateCategoryAsync(Category category)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/category/{category.Id}", category);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteCategoryAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/category/{id}");
        response.EnsureSuccessStatusCode();
    }

    public async Task AddProductsToCategoryAsync(int categoryId, List<int> productIds)
    {
        var payload = new
        {
            CategoryId = categoryId,
            ProductIds = productIds
        };
        var response = await _httpClient.PostAsJsonAsync("api/category/add-products", payload);
        response.EnsureSuccessStatusCode();
    }

    public async Task RemoveProductFromCategoryAsync(int categoryId, int productId)
    {
        var response = await _httpClient.DeleteAsync($"api/category/{categoryId}/product/{productId}");
        response.EnsureSuccessStatusCode();
    }

}




