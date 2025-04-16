using GeneratorStores.DataAccess.Interfaces;

namespace GeneratorStores.DataAccess.Entities;

public class ProductOrder : IEntity<int>
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public string ProductName { get; set; }

    public Product Product { get; set; }

    public int OrderId { get; set; }

    public Order Order { get; set; }

    public double UnitPrice { get; set; }

    public int Amount { get; set; }
}

