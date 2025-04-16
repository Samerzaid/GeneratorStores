using GeneratorStores.DataAccess.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace GeneratorStores.DataAccess.Entities;

public class Review : IEntity<int>
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; } // Reference to the user who submitted the review
    public ApplicationUser User { get; set; }

    [Required]
    public int ProductId { get; set; } // Reference to the reviewed product
    public Product Product { get; set; }

    [Required]
    [Range(1, 5)] // Rating range is 1 to 5 stars
    public int Rating { get; set; }

    public string Comment { get; set; } // Optional comment for the review

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Automatically set creation time
}

