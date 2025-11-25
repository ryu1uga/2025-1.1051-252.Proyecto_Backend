
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Loop.Data;
using Loop.DTOs.Common;
using Loop.Models.Common;
using BCrypt.Net;
using System.Linq;
using System.Threading.Tasks;

namespace Loop.Controller
{
    [ApiController]
    [Route("api/products")]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly ProductDbContext _context;

        public ProductController(ProductDbContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _context.Products
                .OrderBy(p => p.Id)
                .Select(p => new
                {
                    p.Id,
                    p.BusinessId,
                    p.CategoryId,
                    p.Name,
                    p.Description,
                    p.Status,
                    p.Price
                })
                .ToListAsync();

            return Ok(new
            {
                success = true,
                data = products
            });
        }

        // GET: api/Products/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(Guid id)
        {
            var product = await _context.Products
                .Where(p => p.Id == id)
                .Select(p => new 
                {
                    p.Id,
                    p.BusinessId,
                    p.CategoryId,
                    p.Name,
                    p.Description,
                    p.Status,
                    p.Price
                })
                .FirstOrDefaultAsync();

            if (product == null)
                return NotFound(new
                {
                    success = false,
                    data = "Producto no encontrado."
                });

            return Ok(new
            {
                success = true,
                data = product
            });
        }

        // POST: api/Products
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductDTO productDTO)
        {
            var product = new Product
            {
                Id = Guid.NewGuid(),
                BusinessId = productDTO.BusinessId,
                CategoryId = productDTO.CategoryId,
                Name = productDTO.Name,
                Description = productDTO.Description,
                Status = productDTO.Status,
                Price = productDTO.Price,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                data = "Producto creado correctamente."
            });
        }

        // PUT: api/Products/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] ProductDTO productDTO)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound(new
                {
                    success = false,
                    data = "Producto no encontrado."
                });
            }

            product.Name = productDTO.Name;
            product.Description = productDTO.Description;
            product.Status = productDTO.Status;
            product.Price = productDTO.Price;
            product.UpdatedAt = DateTime.UtcNow;

            _context.Products.Update(product);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                data = "Valores actualizados correctamente."
            });
        }

        // DELETE: api/Products
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound(new
                {
                    success = false,
                    data = "Producto no encontrado."
                });
            }

            // Eliminar producto
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                data = "Producto eliminado correctamente."
            });
        }
    }
}
