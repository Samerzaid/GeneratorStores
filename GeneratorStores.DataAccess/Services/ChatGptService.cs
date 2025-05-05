using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;

namespace GeneratorStores.DataAccess.Services;

public class ChatGptService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public ChatGptService(IConfiguration config)
    {
        _httpClient = new HttpClient();
        _apiKey = config["OpenAI:ApiKey"];

        if (string.IsNullOrEmpty(_apiKey))
        {
            throw new Exception("OpenAI API key is missing in appsettings.json");
        }

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
    }


    public async Task<string> AskAsync(string question)
    {
        var request = new
        {
            model = "gpt-3.5-turbo", // or another available model
            messages = new[]
            {
                new { role = "user", content = question }
            }
        };

        var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

        var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);
        var result = await response.Content.ReadAsStringAsync();

        Console.WriteLine("OpenAI Response:");
        Console.WriteLine(result);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("OpenAI API error: " + result);
        }

        var json = JsonDocument.Parse(result);

        if (!json.RootElement.TryGetProperty("choices", out var choices))
        {
            throw new Exception("Missing 'choices' in response: " + result);
        }

        var message = choices[0].GetProperty("message").GetProperty("content").GetString();
        return message ?? "No response from AI.";
    }

}
