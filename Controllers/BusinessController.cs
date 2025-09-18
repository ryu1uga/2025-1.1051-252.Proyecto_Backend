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
    public class BusinessController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BusinessController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Businesses
        [HttpGet]
        public async Task<IActionResult> GetBusinesss()
        {
            var businesss = await _context.Businesses
                .OrderBy(b => b.Id)
                .Select(b => new
                {
                    b.Id,
                    b.Name,
                    b.TaxId,
                    b.State
                })
                .ToListAsync();

            return Ok(new
            {
                success = true,
                data = businesss
            });
        }

        // GET: api/Businesses/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBusiness(Guid id)
        {
            var business = await _context.Businesses
                .Where(b => b.Id == id)
                .Select(b => new 
                {
                    b.Id,
                    b.Name,
                    b.TaxId,
                    b.State
                })
                .FirstOrDefaultAsync();

            if (business == null)
                return NotFound(new
                {
                    success = false,
                    data = "Negocio no encontrado."
                });

            return Ok(new
            {
                success = true,
                data = business
            });
        }

        // POST: api/Businesses
        [HttpPost]
        public async Task<IActionResult> CreateBusiness([FromBody] BusinessDTO businessDTO)
        {
            var business = new Business
            {
                Id = Guid.NewGuid(),
                Name = businessDTO.Name,
                TaxId = businessDTO.TaxId,
                State = businessDTO.State,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Businesses.Add(business);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                data = "Negocio creado correctamente."
            });
        }

        // PUT: api/Businesses/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBusiness(Guid id, [FromBody] BusinessDTO businessDTO)
        {
            var business = await _context.Businesses.FindAsync(id);

            if (business == null)
            {
                return NotFound(new
                {
                    success = false,
                    data = "Negocio no encontrado."
                });
            }

            business.Name = businessDTO.Name;
            business.TaxId = businessDTO.TaxId;
            business.State = businessDTO.State;
            business.UpdatedAt = DateTime.UtcNow;

            _context.Businesses.Update(business);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                data = "Valores actualizados correctamente."
            });
        }

        // DELETE: api/Businesses
        [HttpDelete]
        public async Task<IActionResult> DeleteBusiness(Guid id)
        {
            var business = await _context.Businesses
                .FirstOrDefaultAsync(b => b.Id == id);

            if (business == null)
            {
                return NotFound(new
                {
                    success = false,
                    data = "Negocio no encontrado."
                });
            }

            // Eliminar business
            _context.Businesses.Remove(business);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                data = "Negocio eliminado correctamente."
            });
        }
    }
}