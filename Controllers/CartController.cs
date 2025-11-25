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
    public class CartController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;

        public CartController(ApplicationDbContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
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

        // POST: api/Carts/add-item
        [HttpPost("add-item")]
        public async Task<IActionResult> AddItemToCart([FromBody] AddCartItemDTO request)
        {
            if (request.Quantity <= 0)
            {
                return BadRequest(new
                {
                    success = false,
                    data = "Quantity debe ser mayor a cero."
                });
            }

            var product = await FetchProductAsync(request.ProductId);
            if (product == null)
            {
                return NotFound(new
                {
                    success = false,
                    data = "Producto no encontrado en ProductService."
                });
            }

            var cart = await _context.Carts
                .Include(c => c.CartItems!)
                .FirstOrDefaultAsync(c => c.CustomerId == request.CustomerId);

            if (cart == null)
            {
                cart = new Cart
                {
                    Id = Guid.NewGuid(),
                    CustomerId = request.CustomerId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.Carts.Add(cart);
            }
            else
            {
                cart.UpdatedAt = DateTime.UtcNow;
                _context.Carts.Update(cart);
            }

            var cartItem = new CartItem
            {
                Id = Guid.NewGuid(),
                CartId = cart.Id,
                ProductId = product.Id,
                Quantity = request.Quantity,
                Price = product.Price,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.CartItems.Add(cartItem);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                data = new
                {
                    cartId = cart.Id,
                    cartItemId = cartItem.Id,
                    product = new
                    {
                        product.Id,
                        product.Price
                    }
                }
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
