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
    public class PayoutController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PayoutController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Payouts
        [HttpGet]
        public async Task<IActionResult> GetPayouts()
        {
            var payouts = await _context.Payouts
                .OrderBy(p => p.Id)
                .Select(p => new
                {
                    p.Id,
                    p.BusinessId,
                    p.From,
                    p.To,
                    p.Amount,
                    p.Status
                })
                .ToListAsync();

            return Ok(new
            {
                success = true,
                data = payouts
            });
        }

        // GET: api/Payouts/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPayout(Guid id)
        {
            var payout = await _context.Payouts
                .Where(p => p.Id == id)
                .Select(p => new 
                {
                    p.Id,
                    p.BusinessId,
                    p.From,
                    p.To,
                    p.Amount,
                    p.Status
                })
                .FirstOrDefaultAsync();

            if (payout == null)
                return NotFound(new
                {
                    success = false,
                    data = "Payout no encontrado."
                });

            return Ok(new
            {
                success = true,
                data = payout
            });
        }

        // POST: api/Payouts
        [HttpPost]
        public async Task<IActionResult> CreatePayout([FromBody] PayoutDTO payoutDTO)
        {
            var payout = new Payout
            {
                Id = Guid.NewGuid(),
                BusinessId = payoutDTO.BusinessId,
                From = payoutDTO.From,
                To = payoutDTO.To,
                Amount = payoutDTO.Amount,
                Status = payoutDTO.Status,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Payouts.Add(payout);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                data = "Payout creado correctamente."
            });
        }

        // PUT: api/Payouts/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePayout(Guid id, [FromBody] PayoutDTO payoutDTO)
        {
            var payout = await _context.Payouts.FindAsync(id);

            if (payout == null)
            {
                return NotFound(new
                {
                    success = false,
                    data = "Payout no encontrado."
                });
            }

            payout.From = payoutDTO.From;
            payout.To = payoutDTO.To;
            payout.Amount = payoutDTO.Amount;
            payout.Status = payoutDTO.Status;
            payout.UpdatedAt = DateTime.UtcNow;

            _context.Payouts.Update(payout);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                data = "Valores actualizados correctamente."
            });
        }

        // DELETE: api/Payouts
        [HttpDelete]
        public async Task<IActionResult> DeletePayout(Guid id)
        {
            var payout = await _context.Payouts
                .FirstOrDefaultAsync(p => p.Id == id);

            if (payout == null)
            {
                return NotFound(new
                {
                    success = false,
                    data = "Payout no encontrado."
                });
            }

            // Eliminar payout
            _context.Payouts.Remove(payout);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                data = "Payout eliminado correctamente."
            });
        }
    }
}