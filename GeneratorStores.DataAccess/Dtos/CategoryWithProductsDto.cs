using GeneratorStores.DataAccess.Dtos.ProductDtos;

namespace GeneratorStores.DataAccess.Dtos;

public class CategoryWithProductsDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<ProductDto> Products { get; set; }

}