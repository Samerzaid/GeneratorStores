using GeneratorStores.DataAccess.Dtos;

namespace GeneratorStores.DataAccess.Services;

public interface IChatService
{
    Task<List<ConversationDto>> GetCustomerConversationsAsync(string customerId);
    Task<List<ConversationDto>> GetAdminConversationsAsync();
    Task<List<MessageDto>> GetMessagesByConversationIdAsync(int conversationId, string userId);
    Task SendMessageAsync(MessageDto message);
    Task<int> GetUnreadConversationsCountAsync(string userId);
    Task<ConversationDto> GetOrCreateConversationAsync(string customerId, string adminId);
}
