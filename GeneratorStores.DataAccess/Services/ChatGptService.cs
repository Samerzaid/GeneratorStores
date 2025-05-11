using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using GeneratorStore.DataAccess.Interfaces;

namespace GeneratorStores.DataAccess.Services;

public class ChatGptService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly IProductRepository _productRepository;

    public ChatGptService(IConfiguration config, IProductRepository productRepository)
    {
        _httpClient = new HttpClient();
        _apiKey = config["OpenAI:ApiKey"];
        _productRepository = productRepository;

        if (string.IsNullOrEmpty(_apiKey))
        {
            throw new Exception("OpenAI API key is missing in appsettings.json");
        }

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
    }

    public async Task<string> AskAsync(string question)
    {
        // Check for product-related questions
        var productResponse = await CheckProductQueryAsync(question);
        if (!string.IsNullOrEmpty(productResponse))
        {
            return productResponse;
        }

        // Fallback to OpenAI if no direct product match found
        var request = new
        {
            model = "gpt-3.5-turbo",
            messages = new[]
            {
                    new { role = "user", content = question }
                }
        };

        var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);
        var result = await response.Content.ReadAsStringAsync();

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

    private async Task<string> CheckProductQueryAsync(string question)
    {
        var products = await _productRepository.GetAllAsync();
        var lowerQuestion = question.ToLower();

        foreach (var product in products)
        {
            if (lowerQuestion.Contains(product.ProductName.ToLower()))
            {
                var finalPrice = product.FinalPrice.ToString("C2");
                var discount = product.Discount.HasValue ? $" (Discount: {product.Discount}% off)" : "";
                return $"The price of {product.ProductName} is {finalPrice}{discount}.";
            }
        }

        return string.Empty;
    }
}

