
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Loop.Data;
using Loop.DTOs.Common;
using Loop.Models.Common;
using BCrypt.Net;
using System.Linq;
using System.Threading.Tasks;

namespace Loop.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly ProductDbContext _context;

        public InventoryController(ProductDbContext context)
        {
            _context = context;
        }

        // GET: api/Inventories
        [HttpGet]
        public async Task<IActionResult> GetInventories()
        {
            var Inventories = await _context.Inventories
                .OrderBy(i => i.Id)
                .Select(i => new
                {
                    i.Id,
                    i.ProductId,
                    i.Quantity
                })
                .ToListAsync();

            return Ok(new
            {
                success = true,
                data = Inventories
            });
        }

        // GET: api/Inventories/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetInventory(Guid id)
        {
            var inventory = await _context.Inventories
                .Where(i => i.Id == id)
                .Select(i => new 
                {
                    i.Id,
                    i.ProductId,
                    i.Quantity
                })
                .FirstOrDefaultAsync();

            if (inventory == null)
                return NotFound(new
                {
                    success = false,
                    data = "Inventario no encontrado."
                });

            return Ok(new
            {
                success = true,
                data = inventory
            });
        }

        // POST: api/Inventories
        [HttpPost]
        public async Task<IActionResult> CreateInventory([FromBody] InventoryDTO inventoryDTO)
        {
            var inventory = new Inventory
            {
                Id = Guid.NewGuid(),
                ProductId = inventoryDTO.ProductId,
                Quantity = inventoryDTO.Quantity,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Inventories.Add(inventory);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                data = "Inventario creado correctamente."
            });
        }

        // PUT: api/Inventories/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInventory(Guid id, [FromBody] InventoryDTO inventoryDTO)
        {
            var inventory = await _context.Inventories.FindAsync(id);

            if (inventory == null)
            {
                return NotFound(new
                {
                    success = false,
                    data = "Inventario no encontrado."
                });
            }

            inventory.Quantity = inventoryDTO.Quantity;
            inventory.UpdatedAt = DateTime.UtcNow;

            _context.Inventories.Update(inventory);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                data = "Valores actualizados correctamente."
            });
        }

        // DELETE: api/Inventories
        [HttpDelete]
        public async Task<IActionResult> DeleteInventory(Guid id)
        {
            var inventory = await _context.Inventories
                .FirstOrDefaultAsync(i => i.Id == id);

            if (inventory == null)
            {
                return NotFound(new
                {
                    success = false,
                    data = "Inventario no encontrado."
                });
            }

            // Eliminar inventory
            _context.Inventories.Remove(inventory);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                data = "Inventario eliminado correctamente."
            });
        }
    }
}
