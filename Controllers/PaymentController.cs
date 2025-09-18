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
    public class PaymentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PaymentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Payments
        [HttpGet]
        public async Task<IActionResult> GetPayments()
        {
            var payments = await _context.Payments
                .OrderBy(p => p.Id)
                .Select(p => new
                {
                    p.Id,
                    p.OrderId,
                    p.Provider,
                    p.ProviderRef,
                    p.Amount,
                    p.Status,
                    p.PaidAt
                })
                .ToListAsync();

            return Ok(new
            {
                success = true,
                data = payments
            });
        }

        // GET: api/Payments/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPayment(Guid id)
        {
            var payment = await _context.Payments
                .Where(p => p.Id == id)
                .Select(p => new 
                {
                    p.Id,
                    p.OrderId,
                    p.Provider,
                    p.ProviderRef,
                    p.Amount,
                    p.Status,
                    p.PaidAt
                })
                .FirstOrDefaultAsync();

            if (payment == null)
                return NotFound(new
                {
                    success = false,
                    data = "Pago no encontrado."
                });

            return Ok(new
            {
                success = true,
                data = payment
            });
        }

        // POST: api/Payments
        [HttpPost]
        public async Task<IActionResult> CreatePayment([FromBody] PaymentDTO paymentDTO)
        {
            var payment = new Payment
            {
                Id = Guid.NewGuid(),
                OrderId = paymentDTO.OrderId,
                Provider = paymentDTO.Provider,
                ProviderRef = paymentDTO.ProviderRef,
                Amount = paymentDTO.Amount,
                Status = paymentDTO.Status,
                PaidAt = paymentDTO.PaidAt,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                data = "Pago creado correctamente."
            });
        }

        // PUT: api/Payments/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePayment(Guid id, [FromBody] PaymentDTO paymentDTO)
        {
            var payment = await _context.Payments.FindAsync(id);

            if (payment == null)
            {
                return NotFound(new
                {
                    success = false,
                    data = "Pago no encontrado."
                });
            }

            payment.Provider = paymentDTO.Provider;
            payment.ProviderRef = paymentDTO.ProviderRef;
            payment.Amount = paymentDTO.Amount;
            payment.Status = paymentDTO.Status;
            payment.PaidAt = paymentDTO.PaidAt;
            payment.UpdatedAt = DateTime.UtcNow;

            _context.Payments.Update(payment);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                data = "Valores actualizados correctamente."
            });
        }

        // DELETE: api/Payments
        [HttpDelete]
        public async Task<IActionResult> DeletePayment(Guid id)
        {
            var payment = await _context.Payments
                .FirstOrDefaultAsync(p => p.Id == id);

            if (payment == null)
            {
                return NotFound(new
                {
                    success = false,
                    data = "Pago no encontrado."
                });
            }

            // Eliminar payment
            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                data = "Pago eliminado correctamente."
            });
        }
    }
}