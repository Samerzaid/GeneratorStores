namespace GeneratorStores.DataAccess.Entities;

public class Conversation
{
    public int Id { get; set; }
    public string CustomerId { get; set; } = null!;
    public string AdminId { get; set; } = "f66f0073-5b5c-4e98-b2b8-29d9b1374c76"; // Store/Admin ID

    public ICollection<Message> Messages { get; set; } = new List<Message>();
}


