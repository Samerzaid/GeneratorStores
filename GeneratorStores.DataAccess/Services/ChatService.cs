using GeneratorStores.DataAccess.Dtos;
using System.Net.Http.Json;

namespace GeneratorStores.DataAccess.Services;

public class ChatService : IChatService
{
    private readonly HttpClient _httpClient;

    public ChatService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<ConversationDto>> GetCustomerConversationsAsync(string customerId)
    {
        var result = await _httpClient.GetFromJsonAsync<List<ConversationDto>>($"api/chat/user/{customerId}");
        return result ?? new List<ConversationDto>();
    }

    public async Task<List<ConversationDto>> GetAdminConversationsAsync()
    {
        var result = await _httpClient.GetFromJsonAsync<List<ConversationDto>>("api/chat/admin/conversations");
        return result ?? new List<ConversationDto>();
    }


    public async Task<List<MessageDto>> GetMessagesByConversationIdAsync(int conversationId, string userId)
    {
        var result = await _httpClient.GetFromJsonAsync<List<MessageDto>>(
            $"api/chat/conversation/{conversationId}?userId={userId}");
        return result ?? new List<MessageDto>();
    }

    public async Task SendMessageAsync(MessageDto message)
    {
        var response = await _httpClient.PostAsJsonAsync("api/chat", message);
        response.EnsureSuccessStatusCode();
    }

    public async Task<int> GetUnreadConversationsCountAsync(string userId)
    {
        var result = await _httpClient.GetFromJsonAsync<int>($"api/chat/unreadcount/{userId}");
        return result;
    }

    public async Task<ConversationDto> GetOrCreateConversationAsync(string customerId, string adminId)
    {
        var response = await _httpClient.GetFromJsonAsync<ConversationDto>(
            $"api/chat/getorcreate?customerId={customerId}&adminId={adminId}");

        return response!;
    }



}

