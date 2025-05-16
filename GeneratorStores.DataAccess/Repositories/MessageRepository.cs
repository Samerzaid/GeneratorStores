using GeneratorStores.DataAccess.Entities;
using GeneratorStores.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GeneratorStores.DataAccess.Repositories;

public class MessageRepository(GeneratorDbContext context) : IMessageRepository
{
    public async Task<IEnumerable<Message>> GetMessagesByConversationIdAsync(int conversationId)
    {
        return await context.Messages
            .Where(m => m.ConversationId == conversationId)
            .OrderBy(m => m.Timestamp)
            .ToListAsync();
    }

    public async Task<int> GetUnreadCountAsync(string receiverId)
    {
        return await context.Messages
            .Where(m => m.ReceiverId == receiverId && !m.IsRead)
            .CountAsync();
    }

    public async Task MarkMessagesAsReadAsync(int conversationId, string receiverId)
    {
        var messages = await context.Messages
            .Where(m => m.ConversationId == conversationId && m.ReceiverId == receiverId && !m.IsRead)
            .ToListAsync();

        foreach (var msg in messages)
        {
            msg.IsRead = true;
        }

        await context.SaveChangesAsync();
    }

    public async Task AddMessageAsync(Message message)
    {
        await context.Messages.AddAsync(message);
        await context.SaveChangesAsync(); // Add this line to actually save the message
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }

}


