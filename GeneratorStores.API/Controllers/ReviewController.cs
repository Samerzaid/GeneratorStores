using GeneratorStores.DataAccess.Dtos;
using GeneratorStores.DataAccess.Entities;
using GeneratorStores.DataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GeneratorStores.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReviewController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public ReviewController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    // GET: api/review/details
    [HttpGet("details")]
    public async Task<IActionResult> GetAllReviewsWithDetails()
    {
        var reviews = await _unitOfWork.Reviews.GetAllAsync();
        var reviewDetails = reviews.Select(r => new
        {
            ReviewId = r.Id,
            Rating = r.Rating,
            Comment = r.Comment,
            CreatedAt = r.CreatedAt,
            User = new
            {
                r.UserId,
                r.User.UserName,
                r.User.Email,
                r.User.FullName
            },
            Product = new
            {
                r.ProductId,
                r.Product.ProductName,
                r.Product.Price
            }
        });
        return Ok(reviewDetails);
    }

    // GET: api/review/product/{productId}
    [HttpGet("product/{productId}")]
    public async Task<IActionResult> GetReviewsByProductId(int productId)
    {
        var reviews = await _unitOfWork.Reviews.GetReviewsByProductIdAsync(productId);
        return Ok(reviews);
    }

    // GET: api/review/user/{userId}
    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetReviewsByUserId(string userId)
    {
        var reviews = await _unitOfWork.Reviews.GetReviewsByUserIdAsync(userId);
        return Ok(reviews);
    }

    // POST: api/review
    [HttpPost]
    public async Task<IActionResult> AddReview([FromBody] CreateReviewDto createReviewDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Check if the product exists
        var product = await _unitOfWork.Products.GetByIdAsync(createReviewDto.ProductId);
        if (product == null)
        {
            return NotFound($"Product with ID {createReviewDto.ProductId} not found.");
        }

        // Create a new review
        var review = new Review
        {
            UserId = createReviewDto.UserId,
            ProductId = createReviewDto.ProductId,
            Rating = createReviewDto.Rating,
            Comment = createReviewDto.Comment,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Reviews.AddAsync(review);
        await _unitOfWork.CompleteAsync();

        // Fetch and return the created review with related entities
        var createdReview = await _unitOfWork.Reviews.GetByIdAsync(review.Id);
        return Ok(createdReview);
    }

    // PUT: api/review/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateReview(int id, [FromBody] UpdateReviewDto updateReviewDto)
    {
        if (id != updateReviewDto.Id)
        {
            return BadRequest("Review ID mismatch.");
        }

        var existingReview = await _unitOfWork.Reviews.GetByIdAsync(id);
        if (existingReview == null)
        {
            return NotFound($"Review with ID {id} not found.");
        }

        existingReview.Rating = updateReviewDto.Rating;
        existingReview.Comment = updateReviewDto.Comment;
        await _unitOfWork.CompleteAsync();

        return NoContent();
    }

    // DELETE: api/review/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteReview(int id)
    {
        var review = await _unitOfWork.Reviews.GetByIdAsync(id);
        if (review == null)
        {
            return NotFound($"Review with ID {id} not found.");
        }

        await _unitOfWork.Reviews.DeleteAsync(id);
        await _unitOfWork.CompleteAsync();

        return NoContent();
    }
}

