namespace GeneratorStores.DataAccess.Entities;

public class BestSellingFinal
{
    public int ProductId { get; set; }

    public int? TotalSum { get; set; }

    public string? Title { get; set; }

    public decimal? Price { get; set; }

    public decimal? Discount { get; set; }

    public string? ImageName { get; set; }

    public int? Qty { get; set; }

    public string? Status { get; set; }
}