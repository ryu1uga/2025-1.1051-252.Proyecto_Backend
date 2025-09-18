using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Loop.Data;
using Loop.DTOs.Common;
using Loop.Models.Common;
using Loop.Services;
using BCrypt.Net;

namespace Loop.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ReviewController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Reviews
        [HttpGet]
        public async Task<IActionResult> GetReviews()
        {
            var reviews = await _context.Reviews
                .OrderBy(r => r.Id)
                .Select(r => new
                {
                    r.Id,
                    r.ProductId,
                    r.CustomerId,
                    r.Rating,
                    r.Comment
                })
                .ToListAsync();

            return Ok(new
            {
                success = true,
                data = reviews
            });
        }

        // GET: api/Reviews/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetReview(Guid id)
        {
            var review = await _context.Reviews
                .Where(r => r.Id == id)
                .Select(r => new 
                {
                    r.Id,
                    r.ProductId,
                    r.CustomerId,
                    r.Rating,
                    r.Comment
                })
                .FirstOrDefaultAsync();

            if (review == null)
                return NotFound(new
                {
                    success = false,
                    data = "Reseña no encontrada."
                });

            return Ok(new
            {
                success = true,
                data = review
            });
        }

        // POST: api/Reviews
        [HttpPost]
        public async Task<IActionResult> CreateReview([FromBody] ReviewDTO reviewDTO)
        {
            var review = new Review
            {
                Id = Guid.NewGuid(),
                ProductId = reviewDTO.ProductId,
                CustomerId = reviewDTO.CustomerId,
                Rating = reviewDTO.Rating,
                Comment = reviewDTO.Comment,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                data = "Reseña creada correctamente."
            });
        }

        // PUT: api/Reviews/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReview(Guid id, [FromBody] ReviewDTO reviewDTO)
        {
            var review = await _context.Reviews.FindAsync(id);

            if (review == null)
            {
                return NotFound(new
                {
                    success = false,
                    data = "Reseña no encontrada."
                });
            }

            review.Rating = reviewDTO.Rating;
            review.Comment = reviewDTO.Comment;
            review.UpdatedAt = DateTime.UtcNow;

            _context.Reviews.Update(review);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                data = "Valores actualizados correctamente."
            });
        }

        // DELETE: api/Reviews
        [HttpDelete]
        public async Task<IActionResult> DeleteReview(Guid id)
        {
            var review = await _context.Reviews
                .FirstOrDefaultAsync(r => r.Id == id);

            if (review == null)
            {
                return NotFound(new
                {
                    success = false,
                    data = "Reseña no encontrada."
                });
            }

            // Eliminar reseña
            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                data = "Reseña eliminada correctamente."
            });
        }
    }
}