using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TheLab.Models;

namespace TheLab.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ReviewsController> _logger;

        public ReviewsController(AppDbContext context, ILogger<ReviewsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetAllReviews()
        {
            _logger.LogInformation("Fetching all reviews.");
            var reviews = _context.Reviews.ToList();
            _logger.LogInformation($"Found {reviews.Count} reviews.");
            return Ok(reviews);
        }

        [HttpGet("{id}")]
        public IActionResult GetReviewById(int id)
        {
            _logger.LogInformation($"Fetching review with id {id}.");
            var review = _context.Reviews.FirstOrDefault(r => r.Id == id);
            if (review == null)
            {
                _logger.LogWarning($"Review with id {id} not found.");
                return NotFound();
            }
            _logger.LogInformation($"Found review with id {id}.");
            return Ok(review);
        }

        [HttpPost]
        public IActionResult CreateReview([FromBody] Reviews review)
        {
            if (review == null)
            {
                _logger.LogWarning("Received invalid review data.");
                return BadRequest("Invalid review data.");
            }

            var user = _context.Users.FirstOrDefault(u => u.Id == review.UserId);
            if (user == null)
            {
                _logger.LogWarning($"User with id {review.UserId} not found.");
                return NotFound("User not found.");
            }

            _context.Reviews.Add(review);
            _context.SaveChanges();
            _logger.LogInformation($"Created new review with id {review.Id}.");

            return CreatedAtAction(nameof(GetReviewById), new { id = review.Id }, review);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateReview(int id, [FromBody] Reviews review)
        {
            if (id != review.Id)
            {
                _logger.LogWarning($"Id mismatch. Provided id {id} does not match review id {review.Id}.");
                return BadRequest();
            }

            var user = _context.Users.FirstOrDefault(u => u.Id == review.UserId);
            if (user == null)
            {
                _logger.LogWarning($"User with id {review.UserId} not found.");
                return NotFound("User not found.");
            }

            _context.Entry(review).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            _logger.LogInformation($"Updated review with id {review.Id}.");

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteReview(int id)
        {
            var review = _context.Reviews.FirstOrDefault(r => r.Id == id);
            if (review == null)
            {
                _logger.LogWarning($"Review with id {id} not found.");
                return NotFound();
            }

            var user = _context.Users.FirstOrDefault(u => u.Id == review.UserId);
            if (user == null)
            {
                _logger.LogWarning($"User with id {review.UserId} not found.");
                return NotFound("User not found.");
            }

            _context.Reviews.Remove(review);
            _context.SaveChanges();
            _logger.LogInformation($"Deleted review with id {id}.");

            return NoContent();
        }
    }
}
