using GeneratorStores.DataAccess.Entities;
using GeneratorStores.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GeneratorStores.DataAccess.Repositories;

public class ConversationRepository : IConversationRepository
{
    private readonly GeneratorDbContext _context;

    public ConversationRepository(GeneratorDbContext context)
    {
        _context = context;
    }

    public async Task<Conversation?> GetOrCreateAsync(string customerId, string adminId = "f66f0073-5b5c-4e98-b2b8-29d9b1374c76")
    {
        var convo = await _context.Conversations
            .Include(c => c.Messages)
            .FirstOrDefaultAsync(c => c.CustomerId == customerId && c.AdminId == adminId);

        if (convo == null)
        {
            convo = new Conversation
            {
                CustomerId = customerId,
                AdminId = adminId
            };

            _context.Conversations.Add(convo);
            await _context.SaveChangesAsync();
        }

        return convo;
    }


    public async Task<IEnumerable<Conversation>> GetAdminConversationsAsync()
    {
        return await _context.Conversations
            .Include(c => c.Messages)
            .Where(c => c.AdminId == "f66f0073-5b5c-4e98-b2b8-29d9b1374c76")
            .ToListAsync();
    }

    public async Task<IEnumerable<Conversation>> GetCustomerConversationsAsync(string customerId)
    {
        return await _context.Conversations
            .Include(c => c.Messages)
            .Where(c => c.CustomerId == customerId)
            .ToListAsync();
    }

    public async Task<Conversation?> GetByIdAsync(int conversationId)
    {
        return await _context.Conversations
            .Include(c => c.Messages)
            .FirstOrDefaultAsync(c => c.Id == conversationId);
    }


}



