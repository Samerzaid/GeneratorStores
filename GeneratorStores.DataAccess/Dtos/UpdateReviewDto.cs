using System.ComponentModel.DataAnnotations;

namespace GeneratorStores.DataAccess.Dtos;

public class UpdateReviewDto
{
    public int Id { get; set; } // Review ID
    [Range(1, 5)]
    public int Rating { get; set; }
    public string Comment { get; set; }

}