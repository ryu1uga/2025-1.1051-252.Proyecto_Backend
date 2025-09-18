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
    public class ProductImageController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductImageController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ProductImages
        [HttpGet]
        public async Task<IActionResult> GetProductImages()
        {
            var productImages = await _context.ProductImages
                .OrderBy(pi => pi.Id)
                .Select(pi => new
                {
                    pi.Id,
                    pi.ProductId,
                    pi.Url,
                    pi.Order
                })
                .ToListAsync();

            return Ok(new
            {
                success = true,
                data = productImages
            });
        }

        // GET: api/ProductImages/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductImage(Guid id)
        {
            var productImage = await _context.ProductImages
                .Where(pi => pi.Id == id)
                .Select(pi => new 
                {
                    pi.Id,
                    pi.ProductId,
                    pi.Url,
                    pi.Order
                })
                .FirstOrDefaultAsync();

            if (productImage == null)
                return NotFound(new
                {
                    success = false,
                    data = "Imagen de producto no encontrado."
                });

            return Ok(new
            {
                success = true,
                data = productImage
            });
        }

        // POST: api/ProductImages
        [HttpPost]
        public async Task<IActionResult> CreateProductImage([FromBody] ProductImageDTO productImageDTO)
        {
            var productImage = new ProductImage
            {
                Id = Guid.NewGuid(),
                ProductId = productImageDTO.ProductId,
                Url = productImageDTO.Url,
                Order = productImageDTO.Order,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.ProductImages.Add(productImage);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                data = "Imagen de producto creado correctamente."
            });
        }

        // PUT: api/ProductImages/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProductImage(Guid id, [FromBody] ProductImageDTO productImageDTO)
        {
            var productImage = await _context.ProductImages.FindAsync(id);

            if (productImage == null)
            {
                return NotFound(new
                {
                    success = false,
                    data = "Imagen de producto no encontrado."
                });
            }

            productImage.Url = productImageDTO.Url;
            productImage.Order = productImageDTO.Order;
            productImage.UpdatedAt = DateTime.UtcNow;

            _context.ProductImages.Update(productImage);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                data = "Valores actualizados correctamente."
            });
        }

        // DELETE: api/ProductImages
        [HttpDelete]
        public async Task<IActionResult> DeleteProductImage(Guid id)
        {
            var productImage = await _context.ProductImages
                .FirstOrDefaultAsync(pi => pi.Id == id);

            if (productImage == null)
            {
                return NotFound(new
                {
                    success = false,
                    data = "Imagen de producto no encontrado."
                });
            }

            // Eliminar imagen de producto
            _context.ProductImages.Remove(productImage);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                data = "Imagen de producto eliminado correctamente."
            });
        }
    }
}