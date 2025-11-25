
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Loop.Data;
using Loop.DTOs.Common;
using Loop.Models.Common;
using Loop.Services;
using System.Linq;
using System.Threading.Tasks;

namespace Loop.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Carts
        [HttpGet]
        public async Task<IActionResult> GetCarts()
        {
            var Carts = await _context.Carts
                .OrderBy(c => c.Id)
                .Select(c => new
                {
                    c.Id,
                    c.CustomerId
                })
                .ToListAsync();

            return Ok(new
            {
                success = true,
                data = Carts
            });
        }

        // GET: api/Carts/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCart(Guid id)
        {
            var cart = await _context.Carts
                .Where(c => c.Id == id)
                .Select(c => new 
                {
                    c.Id,
                    c.CustomerId
                })
                .FirstOrDefaultAsync();

            if (cart == null)
                return NotFound(new
                {
                    success = false,
                    data = "Carrito de compra no encontrado."
                });

            return Ok(new
            {
                success = true,
                data = cart
            });
        }

        // POST: api/Carts
        [HttpPost]
        public async Task<IActionResult> CreateCart([FromBody] CartDTO cartDTO)
        {
            var cart = new Cart
            {
                Id = Guid.NewGuid(),
                CustomerId = cartDTO.CustomerId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                data = "Carrito de compra creado correctamente."
            });
        }
        
        // DELETE: api/Carts
        [HttpDelete]
        public async Task<IActionResult> DeleteCart(Guid id)
        {
            var cart = await _context.Carts
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cart == null)
            {
                return NotFound(new
                {
                    success = false,
                    data = "Carrito de compra no encontrado."
                });
            }

            // Eliminar cart
            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                data = "Carrito de compra eliminado correctamente."
            });
        }
    }
}