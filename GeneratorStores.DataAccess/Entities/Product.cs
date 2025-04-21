using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using GeneratorStores.DataAccess.Interfaces;

namespace GeneratorStores.DataAccess.Entities;

public class Product : IEntity<int>
{
        [Key]
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public double Price { get; set; }
        public int AmountOfProduct { get; set; }
        public bool State { get; set; }
        public decimal? Discount { get; set; }
        public string? ImageName { get; set; }

        [JsonIgnore]
        public double FinalPrice => Discount.HasValue
            ? Price - (Price * (double)(Discount.Value / 100))
            : Price;

}


