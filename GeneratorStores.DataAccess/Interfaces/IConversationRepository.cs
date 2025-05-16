using GeneratorStores.DataAccess.Entities;

namespace GeneratorStores.DataAccess.Interfaces;

public interface IConversationRepository
{
    Task<Conversation?> GetOrCreateAsync(string customerId, string adminId = "f66f0073-5b5c-4e98-b2b8-29d9b1374c76");
    Task<IEnumerable<Conversation>> GetCustomerConversationsAsync(string customerId);
    Task<IEnumerable<Conversation>> GetAdminConversationsAsync();
    Task<Conversation?> GetByIdAsync(int conversationId);
}


