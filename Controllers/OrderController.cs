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
    public class OrderController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _context.Orders
                .OrderBy(o => o.Id)
                .Select(o => new
                {
                    o.Id,
                    o.CustomerId,
                    o.AddressId,
                    o.TotalAmount,
                    o.Status,
                    o.PaymentStatus,
                    o.ShippingStatus
                })
                .ToListAsync();

            return Ok(new
            {
                success = true,
                data = orders
            });
        }

        // GET: api/Orders/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(Guid id)
        {
            var order = await _context.Orders
                .Where(o => o.Id == id)
                .Select(o => new 
                {
                    o.Id,
                    o.CustomerId,
                    o.AddressId,
                    o.TotalAmount,
                    o.Status,
                    o.PaymentStatus,
                    o.ShippingStatus
                })
                .FirstOrDefaultAsync();

            if (order == null)
                return NotFound(new
                {
                    success = false,
                    data = "Pedido no encontrado."
                });

            return Ok(new
            {
                success = true,
                data = order
            });
        }

        // POST: api/Orders
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderDTO orderDTO)
        {
            var order = new Order
            {
                Id = Guid.NewGuid(),
                CustomerId = orderDTO.CustomerId,
                AddressId = orderDTO.AddressId,
                TotalAmount = orderDTO.TotalAmount,
                Status = orderDTO.Status,
                PaymentStatus = orderDTO.PaymentStatus,
                ShippingStatus = orderDTO.ShippingStatus,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                data = "Pedido creado correctamente."
            });
        }

        // PUT: api/Orders/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(Guid id, [FromBody] OrderDTO orderDTO)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound(new
                {
                    success = false,
                    data = "Pedido no encontrado."
                });
            }

            order.TotalAmount = orderDTO.TotalAmount;
            order.Status = orderDTO.Status;
            order.PaymentStatus = orderDTO.PaymentStatus;
            order.ShippingStatus = orderDTO.ShippingStatus;
            order.UpdatedAt = DateTime.UtcNow;

            _context.Orders.Update(order);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                data = "Valores actualizados correctamente."
            });
        }

        // DELETE: api/Orders
        [HttpDelete]
        public async Task<IActionResult> DeleteOrder(Guid id)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound(new
                {
                    success = false,
                    data = "Pedido no encontrado."
                });
            }

            // Eliminar order
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                data = "Pedido eliminado correctamente."
            });
        }
    }
}