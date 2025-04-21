using GeneratorStores.DataAccess.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace GeneratorStores.DataAccess.Entities;

public class Review : IEntity<int>
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; } 
    public ApplicationUser User { get; set; }

    [Required]
    public int ProductId { get; set; } 
    public Product Product { get; set; }

    [Required]
    [Range(1, 5)] // Rating range is 1 to 5 stars
    public int Rating { get; set; }

    public string Comment { get; set; } 

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; 
}


