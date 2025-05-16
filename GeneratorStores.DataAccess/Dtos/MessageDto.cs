namespace GeneratorStores.DataAccess.Dtos;

public class MessageDto
{
    public int ConversationId { get; set; }
    public string SenderId { get; set; } = null!;
    public string ReceiverId { get; set; } = null!;
    public string Content { get; set; } = null!;
    public DateTime Timestamp { get; set; }
}


