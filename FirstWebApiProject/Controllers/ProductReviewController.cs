using FirstWebApiProject.DTO.Order;
using FirstWebApiProject.DTO.ProductReview;
using FirstWebApiProject.Interfaces;
using FirstWebApiProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace FirstWebApiProject.Controllers
{
    [Route("Product/[controller]")]
    [ApiController]
    public class ProductReviewController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public ProductReviewController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpGet("{id:int}")]
        public IActionResult GetReviewById(int id)
        {
            var review = unitOfWork.Repositry<ProductReview>().GetById(id);
            if (review == null)
            {
                return NotFound($"Review with ID {id} not found.");
            }

            var reviewDto = new ProductReviewDTO
            {
                Id=review.Id,
                Rating = review.Rating,
                Comment = review.Comment,
                ReviewDate=DateTime.Now
            };

            return Ok(reviewDto);
        }
        [HttpPost]
        public IActionResult CreateReview(ProductReviewDTO productReviewDTO)
        {
            if (ModelState.IsValid)
            {
                var ProductView = new ProductReview() { Id=productReviewDTO.Id , Rating = productReviewDTO.Rating , Comment=productReviewDTO.Comment,ReviewDate=DateTime.Now };
                try
                {
                    unitOfWork.Repositry<ProductReview>().Add(ProductView);
                    unitOfWork.Complete();
                    var ProductViewView = new ProductReviewDTO() { Id = ProductView.Id, Rating = ProductView.Rating, Comment = ProductView.Comment,ReviewDate=DateTime.Now };
                    return CreatedAtAction(nameof(GetReviewById), new { id = ProductView.Id }, ProductViewView);

                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }

            }
            return BadRequest(ModelState);
        }
       
        [HttpGet("{productId}")]
        public IActionResult GetReviewsByProductId(int productId)
            {
                // Step 1: Query reviews for the given product ID
                var reviews = unitOfWork.Repositry<ProductReview>()
                    .GetAll()
                    .Where(r => r.ProductID == productId)
                    .ToList();

                // Step 2: Handle the case where no reviews exist for the product
                if (!reviews.Any())
                {
                    return NotFound($"No reviews found for Product ID {productId}.");
                }

                // Step 3: Map to anonymous objects or DTOs without the Id
                var reviewResponses = reviews.Select(r => new
                {
                    Rating = r.Rating,
                    Comment = r.Comment,
                    ReviewDate = r.ReviewDate
                }).ToList();

                // Step 4: Return the reviews
                return Ok(reviewResponses);
            }

        
    }
}
