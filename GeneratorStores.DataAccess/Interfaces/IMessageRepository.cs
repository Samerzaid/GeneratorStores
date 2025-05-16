using GeneratorStores.DataAccess.Entities;

namespace GeneratorStores.DataAccess.Interfaces;

public interface IMessageRepository
{
    Task<IEnumerable<Message>> GetMessagesByConversationIdAsync(int conversationId);
    Task<int> GetUnreadCountAsync(string receiverId);
    Task MarkMessagesAsReadAsync(int conversationId, string receiverId);
    Task AddMessageAsync(Message message);
    Task SaveChangesAsync(); // Add this to ensure messages are saved correctly
}



