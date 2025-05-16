namespace GeneratorStores.DataAccess.Dtos;

public class ConversationDto
{
    public int ConversationId { get; set; }
    public string UserId { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string? LastMessage { get; set; }
    public DateTime? LastTimestamp { get; set; }
    public bool HasUnreadMessages { get; set; }
    public int UnreadCount { get; set; }
}
