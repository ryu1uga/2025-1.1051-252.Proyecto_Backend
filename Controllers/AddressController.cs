
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
    public class AddressController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AddressController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Addresses
        [HttpGet]
        public async Task<IActionResult> GetAddresss()
        {
            var addresss = await _context.Addresses
                .OrderBy(a => a.Id)
                .Select(a => new
                {
                    a.Id,
                    a.CustomerId,
                    a.Line1,
                    a.Line2,
                    a.City,
                    a.Region,
                    a.PostalCode,
                    a.Country
                })
                .ToListAsync();

            return Ok(new
            {
                success = true,
                data = addresss
            });
        }

        // GET: api/Addresses/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAddress(Guid id)
        {
            var address = await _context.Addresses
                .Where(a => a.Id == id)
                .Select(a => new 
                {
                    a.Id,
                    a.CustomerId,
                    a.Line1,
                    a.Line2,
                    a.City,
                    a.Region,
                    a.PostalCode,
                    a.Country
                })
                .FirstOrDefaultAsync();

            if (address == null)
                return NotFound(new
                {
                    success = false,
                    data = "Negocio no encontrado."
                });

            return Ok(new
            {
                success = true,
                data = address
            });
        }

        // POST: api/Addresses
        [HttpPost]
        public async Task<IActionResult> CreateAddress([FromBody] AddressDTO addressDTO)
        {
            var address = new Address
            {
                Id = Guid.NewGuid(),
                CustomerId = addressDTO.CustomerId,
                Line1 = addressDTO.Line1,
                Line2 = addressDTO.Line2,
                City = addressDTO.City,
                Region = addressDTO.Region,
                PostalCode = addressDTO.PostalCode,
                Country = addressDTO.Country,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Addresses.Add(address);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                data = "Negocio creado correctamente."
            });
        }

        // PUT: api/Addresses/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAddress(Guid id, [FromBody] AddressDTO addressDTO)
        {
            var address = await _context.Addresses.FindAsync(id);

            if (address == null)
            {
                return NotFound(new
                {
                    success = false,
                    data = "Negocio no encontrado."
                });
            }

            address.Line1 = addressDTO.Line1;
            address.Line2 = addressDTO.Line2;
            address.City = addressDTO.City;
            address.Region = addressDTO.Region;
            address.PostalCode = addressDTO.PostalCode;
            address.Country = addressDTO.Country;
            address.UpdatedAt = DateTime.UtcNow;

            _context.Addresses.Update(address);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                data = "Valores actualizados correctamente."
            });
        }

        // DELETE: api/Addresses
        [HttpDelete]
        public async Task<IActionResult> DeleteAddress(Guid id)
        {
            var address = await _context.Addresses
                .FirstOrDefaultAsync(a => a.Id == id);

            if (address == null)
            {
                return NotFound(new
                {
                    success = false,
                    data = "Negocio no encontrado."
                });
            }

            // Eliminar address
            _context.Addresses.Remove(address);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                data = "Negocio eliminado correctamente."
            });
        }
    }
}