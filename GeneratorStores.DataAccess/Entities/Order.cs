using System.ComponentModel.DataAnnotations;
using GeneratorStores.DataAccess.Interfaces;

namespace GeneratorStores.DataAccess.Entities;

public class Order : IEntity<int>
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; }

    [Required]
    public double TotalPrice { get; set; }

    public DateTime OrderDate { get; set; } = DateTime.UtcNow;

    public ICollection<ProductOrder> ProductsInOrder { get; set; } = new List<ProductOrder>();


    public ApplicationUser User { get; set; }

    public string? TransId { get; set; }

    public string? Status { get; set; } 

}



