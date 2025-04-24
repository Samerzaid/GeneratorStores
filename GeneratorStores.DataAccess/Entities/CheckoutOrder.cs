using System.ComponentModel.DataAnnotations;

namespace GeneratorStores.DataAccess.Entities;

public class CheckoutOrder
{
    [Required]
    public string OrderNumber { get; set; }
    [Required]
    public string Name { get; set; }
    [Required, EmailAddress]
    public string Email { get; set; }
    [Required]
    public List<Product> Products { get; set; } = new List<Product>();
    [Required]
    public double TotalPrice { get; set; }
}



