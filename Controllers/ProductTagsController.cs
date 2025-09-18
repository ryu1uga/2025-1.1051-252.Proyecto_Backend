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
    public class ProductTagController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductTagController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ProductTags
        [HttpGet]
        public async Task<IActionResult> GetProductTags()
        {
            var productTags = await _context.ProductTags
                .OrderBy(pt => pt.Id)
                .Select(pt => new
                {
                    pt.Id,
                    pt.ProductId,
                    pt.TagId
                })
                .ToListAsync();

            return Ok(new
            {
                success = true,
                data = productTags
            });
        }

        // GET: api/ProductTags/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductTag(Guid id)
        {
            var productTag = await _context.ProductTags
                .Where(pt => pt.Id == id)
                .Select(pt => new 
                {
                    pt.Id,
                    pt.ProductId,
                    pt.TagId
                })
                .FirstOrDefaultAsync();

            if (productTag == null)
                return NotFound(new
                {
                    success = false,
                    data = "Relación entre producto y etiqueta no encontrada."
                });

            return Ok(new
            {
                success = true,
                data = productTag
            });
        }

        // POST: api/ProductTags
        [HttpPost]
        public async Task<IActionResult> CreateProductTag([FromBody] ProductTagDTO productTagDTO)
        {
            var productTag = new ProductTag
            {
                Id = Guid.NewGuid(),
                ProductId = productTagDTO.ProductId,
                TagId = productTagDTO.TagId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.ProductTags.Add(productTag);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                data = "Relación entre producto y etiqueta creada correctamente."
            });
        }

        // DELETE: api/ProductTags
        [HttpDelete]
        public async Task<IActionResult> DeleteProductTag(Guid id)
        {
            var productTag = await _context.ProductTags
                .FirstOrDefaultAsync(pt => pt.Id == id);

            if (productTag == null)
            {
                return NotFound(new
                {
                    success = false,
                    data = "Relación entre producto y etiqueta no encontrada."
                });
            }

            // Eliminar relación entre producto y etiqueta
            _context.ProductTags.Remove(productTag);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                data = "Relación entre producto y etiqueta eliminada correctamente."
            });
        }
    }
}