namespace GeneratorStores.DataAccess.Dtos.ProductDtos;

public class ProductDto
{
    public int Id { get; set; }
    public string ProductName { get; set; }
    public string ProductDescription { get; set; }
    public double Price { get; set; }
    public int AmountOfProduct { get; set; }
    public bool State { get; set; }
}