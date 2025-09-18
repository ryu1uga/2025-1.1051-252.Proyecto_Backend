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
    public class CustomerController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CustomerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Customers
        [HttpGet]
        public async Task<IActionResult> GetCustomers()
        {
            var Customers = await _context.Customers
                .OrderBy(c => c.Id)
                .Select(c => new
                {
                    c.Id,
                    c.UserId
                })
                .ToListAsync();

            return Ok(new
            {
                success = true,
                data = Customers
            });
        }

        // GET: api/Customers/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomer(Guid id)
        {
            var customer = await _context.Customers
                .Where(c => c.Id == id)
                .Select(c => new 
                {
                    c.Id,
                    c.UserId
                })
                .FirstOrDefaultAsync();

            if (customer == null)
                return NotFound(new
                {
                    success = false,
                    data = "Cliente no encontrado."
                });

            return Ok(new
            {
                success = true,
                data = customer
            });
        }

        // POST: api/Customers
        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody] CustomerDTO customerDTO)
        {
            var customer = new Customer
            {
                Id = Guid.NewGuid(),
                UserId = customerDTO.UserId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                data = "Cliente creado correctamente."
            });
        }
        
        // DELETE: api/Customers
        [HttpDelete]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.Id == id);

            if (customer == null)
            {
                return NotFound(new
                {
                    success = false,
                    data = "Cliente no encontrado."
                });
            }

            // Eliminar customer
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                data = "Cliente eliminado correctamente."
            });
        }
    }
}