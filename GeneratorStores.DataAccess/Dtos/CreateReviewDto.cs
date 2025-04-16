using System.ComponentModel.DataAnnotations;

namespace GeneratorStores.DataAccess.Dtos;

public class CreateReviewDto
{
    public string UserId { get; set; }
    public int ProductId { get; set; }
    [Range(1, 5)]
    public int Rating { get; set; }
    public string Comment { get; set; }

}

