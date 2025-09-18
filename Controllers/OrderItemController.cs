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
    public class OrderItemController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OrderItemController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/OrderItems
        [HttpGet]
        public async Task<IActionResult> GetOrderItems()
        {
            var orderItems = await _context.OrderItems
                .OrderBy(oi => oi.Id)
                .Select(oi => new
                {
                    oi.Id,
                    oi.OrderId,
                    oi.BusinessId,
                    oi.ProductId,
                    oi.Quantity,
                    oi.Price,
                    oi.PaymentStatus
                })
                .ToListAsync();

            return Ok(new
            {
                success = true,
                data = orderItems
            });
        }

        // GET: api/OrderItems/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderItem(Guid id)
        {
            var orderItem = await _context.OrderItems
                .Where(oi => oi.Id == id)
                .Select(oi => new 
                {
                    oi.Id,
                    oi.OrderId,
                    oi.BusinessId,
                    oi.ProductId,
                    oi.Quantity,
                    oi.Price,
                    oi.PaymentStatus
                })
                .FirstOrDefaultAsync();

            if (orderItem == null)
                return NotFound(new
                {
                    success = false,
                    data = "Item del pedido no encontrado."
                });

            return Ok(new
            {
                success = true,
                data = orderItem
            });
        }

        // POST: api/OrderItems
        [HttpPost]
        public async Task<IActionResult> CreateOrderItem([FromBody] OrderItemDTO orderItemDTO)
        {
            var orderItem = new OrderItem
            {
                Id = Guid.NewGuid(),
                OrderId = orderItemDTO.OrderId,
                BusinessId = orderItemDTO.BusinessId,
                ProductId = orderItemDTO.ProductId,
                Quantity = orderItemDTO.Quantity,
                Price = orderItemDTO.Price,
                PaymentStatus = orderItemDTO.PaymentStatus,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.OrderItems.Add(orderItem);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                data = "Item del pedido creado correctamente."
            });
        }

        // PUT: api/OrderItems/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrderItem(Guid id, [FromBody] OrderItemDTO orderItemDTO)
        {
            var orderItem = await _context.OrderItems.FindAsync(id);

            if (orderItem == null)
            {
                return NotFound(new
                {
                    success = false,
                    data = "Item del pedido no encontrado."
                });
            }

            orderItem.Quantity = orderItemDTO.Quantity;
            orderItem.Price = orderItemDTO.Price;
            orderItem.PaymentStatus = orderItemDTO.PaymentStatus;
            orderItem.UpdatedAt = DateTime.UtcNow;

            _context.OrderItems.Update(orderItem);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                data = "Valores actualizados correctamente."
            });
        }

        // DELETE: api/OrderItems
        [HttpDelete]
        public async Task<IActionResult> DeleteOrderItem(Guid id)
        {
            var orderItem = await _context.OrderItems
                .FirstOrDefaultAsync(oi => oi.Id == id);

            if (orderItem == null)
            {
                return NotFound(new
                {
                    success = false,
                    data = "Item del pedido no encontrado."
                });
            }

            // Eliminar orderItem
            _context.OrderItems.Remove(orderItem);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                data = "Item del pedido eliminado correctamente."
            });
        }
    }
}