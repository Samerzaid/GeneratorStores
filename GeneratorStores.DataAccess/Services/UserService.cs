using GeneratorStores.DataAccess.Entities;
using System.Net.Http.Json;

namespace GeneratorStores.DataAccess.Services;

public class UserService
{
    private readonly HttpClient _httpClient;

    public UserService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<ApplicationUser>> GetAllUsersAsync()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<ApplicationUser>>("api/Users");
    }

    public async Task<ApplicationUser> GetUserByIdAsync(string id)
    {
        return await _httpClient.GetFromJsonAsync<ApplicationUser>($"api/Users/{id}");
    }

    public async Task CreateUserAsync(ApplicationUser user)
    {
        var response = await _httpClient.PostAsJsonAsync("api/Users", user);
        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateUserAsync(string id, ApplicationUser user)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/Users/{id}", user);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteUserAsync(string id)
    {
        var response = await _httpClient.DeleteAsync($"api/Users/{id}");
        response.EnsureSuccessStatusCode();
    }
}
