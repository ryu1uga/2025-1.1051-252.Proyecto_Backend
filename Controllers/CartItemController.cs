
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Loop.Data;
using Loop.DTOs.Common;
using Loop.Models.Common;
using Loop.Services;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
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
        private readonly IHttpClientFactory _httpClientFactory;

        public CartItemController(ApplicationDbContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
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

            var product = await FetchProductAsync(cartItemDTO.ProductId);
            if (product == null)
            {
                return NotFound(new
                {
                    success = false,
                    data = "Producto no encontrado en ProductService."
                });
            }

            var cartItem = new CartItem
            {
                Id = Guid.NewGuid(),
                CartId = cartItemDTO.CartId,
                ProductId = cartItemDTO.ProductId,
                Quantity = cartItemDTO.Quantity,
                Price = product.Price,
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

            var product = await FetchProductAsync(cartItemDTO.ProductId);
            if (product == null)
            {
                return NotFound(new
                {
                    success = false,
                    data = "Producto no encontrado en ProductService."
                });
            }

            cartItem.Quantity = cartItemDTO.Quantity;
            cartItem.Price = product.Price;
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

        private async Task<ProductData?> FetchProductAsync(Guid productId)
        {
            var client = _httpClientFactory.CreateClient("ProductService");
            using var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"api/Product/{productId}");

            var authHeader = Request.Headers["Authorization"].FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(authHeader))
            {
                httpRequest.Headers.Authorization = AuthenticationHeaderValue.Parse(authHeader);
            }

            var response = await client.SendAsync(httpRequest);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var payload = await response.Content.ReadFromJsonAsync<ProductApiResponse>();
            if (payload == null || payload.success != true || payload.data == null)
            {
                return null;
            }

            return payload.data;
        }
    }
}
