using GeneratorStores.DataAccess.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace GeneratorStores.DataAccess.Entities;

public class CategoryProduct : IEntity<int>
{
    [Key]
    public int Id { get; set; }

    public int CategoryId { get; set; }
    public Category Category { get; set; }

    public int ProductId { get; set; }
    public Product Product { get; set; }
}

