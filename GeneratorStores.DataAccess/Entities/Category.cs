using System.ComponentModel.DataAnnotations;
using GeneratorStores.DataAccess.Interfaces;

namespace GeneratorStores.DataAccess.Entities;

public class Category : IEntity<int>
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }


}

