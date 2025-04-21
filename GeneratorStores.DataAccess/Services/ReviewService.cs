using GeneratorStores.DataAccess.Dtos;
using System.Net.Http.Json;
using GeneratorStores.DataAccess.Entities;

namespace GeneratorStores.DataAccess.Services;

public class ReviewService : IReviewService
{
    private readonly HttpClient _httpClient;

    public ReviewService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<Review>> GetAllReviewsAsync()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<Review>>("api/review/details") ?? new List<Review>();
    }

    public async Task<IEnumerable<Review>> GetReviewsByProductIdAsync(int productId)
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<Review>>($"api/review/product/{productId}") ?? new List<Review>();
    }


    public async Task<IEnumerable<Review>> GetReviewsByUserIdAsync(string userId)
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<Review>>($"api/review/user/{userId}");
    }

    public async Task AddReviewAsync(Review review)
    {
        var response = await _httpClient.PostAsJsonAsync("api/review", review);
        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateReviewAsync(Review review)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/review/{review.Id}", review);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteReviewAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/review/{id}");
        response.EnsureSuccessStatusCode();
    }
}


