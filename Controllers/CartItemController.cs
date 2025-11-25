
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
    public class CartItemController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CartItemController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/CartItems
        [HttpGet]
        public async Task<IActionResult> GetCartItems()
        {
            var cartItems = await _context.CartItems
                .OrderBy(ci => ci.Id)
                .Select(ci => new
                {
                    ci.Id,
                    ci.CartId,
                    ci.ProductId,
                    ci.Quantity,
                    ci.Price
                })
                .ToListAsync();

            return Ok(new
            {
                success = true,
                data = cartItems
            });
        }

        // GET: api/CartItems/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCartItem(Guid id)
        {
            var cartItem = await _context.CartItems
                .Where(ci => ci.Id == id)
                .Select(ci => new 
                {
                    ci.Id,
                    ci.CartId,
                    ci.ProductId,
                    ci.Quantity,
                    ci.Price
                })
                .FirstOrDefaultAsync();

            if (cartItem == null)
                return NotFound(new
                {
                    success = false,
                    data = "Item del carrito de compra no encontrado."
                });

            return Ok(new
            {
                success = true,
                data = cartItem
            });
        }

        // POST: api/CartItems
        [HttpPost]
        public async Task<IActionResult> CreateCartItem([FromBody] CartItemDTO cartItemDTO)
        {
            if (cartItemDTO.Quantity <= 0)
            {
                return BadRequest(new
                {
                    success = false,
                    data = "Quantity debe ser mayor a cero."
                });
            }

            if (cartItemDTO.Price <= 0)
            {
                return BadRequest(new
                {
                    success = false,
                    data = "Price debe ser mayor a cero."
                });
            }

            var cartItem = new CartItem
            {
                Id = Guid.NewGuid(),
                CartId = cartItemDTO.CartId,
                ProductId = cartItemDTO.ProductId,
                Quantity = cartItemDTO.Quantity,
                Price = cartItemDTO.Price,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.CartItems.Add(cartItem);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                data = "Item del carrito de compra creado correctamente."
            });
        }

        // PUT: api/CartItems/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCartItem(Guid id, [FromBody] CartItemDTO cartItemDTO)
        {
            var cartItem = await _context.CartItems.FindAsync(id);

            if (cartItem == null)
            {
                return NotFound(new
                {
                    success = false,
                    data = "Item del carrito de compra no encontrado."
                });
            }

            if (cartItemDTO.Quantity <= 0)
            {
                return BadRequest(new
                {
                    success = false,
                    data = "Quantity debe ser mayor a cero."
                });
            }

            if (cartItemDTO.Price <= 0)
            {
                return BadRequest(new
                {
                    success = false,
                    data = "Price debe ser mayor a cero."
                });
            }

            cartItem.Quantity = cartItemDTO.Quantity;
            cartItem.Price = cartItemDTO.Price;
            cartItem.UpdatedAt = DateTime.UtcNow;

            _context.CartItems.Update(cartItem);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                data = "Valores actualizados correctamente."
            });
        }

        // DELETE: api/CartItems
        [HttpDelete]
        public async Task<IActionResult> DeleteCartItem(Guid id)
        {
            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.Id == id);

            if (cartItem == null)
            {
                return NotFound(new
                {
                    success = false,
                    data = "Item del carrito de compra no encontrado."
                });
            }

            // Eliminar cartItem
            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                data = "Item del carrito de compra eliminado correctamente."
            });
        }
    }
}
