
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
    [Route("api/tags")]
    [Authorize]
    public class TagController : ControllerBase
    {
        private readonly ProductDbContext _context;

        public TagController(ProductDbContext context)
        {
            _context = context;
        }

        // GET: api/Tags
        [HttpGet]
        public async Task<IActionResult> GetTags()
        {
            var tags = await _context.Tags
                .OrderBy(t => t.Id)
                .Select(t => new
                {
                    t.Id,
                    t.Name
                })
                .ToListAsync();

            return Ok(new
            {
                success = true,
                data = tags
            });
        }

        // GET: api/Tags/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTag(Guid id)
        {
            var tag = await _context.Tags
                .Where(t => t.Id == id)
                .Select(t => new 
                {
                    t.Id,
                    t.Name
                })
                .FirstOrDefaultAsync();

            if (tag == null)
                return NotFound(new
                {
                    success = false,
                    data = "Etiqueta no encontrada."
                });

            return Ok(new
            {
                success = true,
                data = tag
            });
        }

        // POST: api/Tags
        [HttpPost]
        public async Task<IActionResult> CreateTag([FromBody] TagDTO tagDTO)
        {
            var tag = new Tag
            {
                Id = Guid.NewGuid(),
                Name = tagDTO.Name,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Tags.Add(tag);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                data = "Etiqueta creada correctamente."
            });
        }

        // PUT: api/Tags/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTag(Guid id, [FromBody] TagDTO tagDTO)
        {
            var tag = await _context.Tags.FindAsync(id);

            if (tag == null)
            {
                return NotFound(new
                {
                    success = false,
                    data = "Etiqueta no encontrada."
                });
            }

            tag.Name = tagDTO.Name;
            tag.UpdatedAt = DateTime.UtcNow;

            _context.Tags.Update(tag);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                data = "Valores actualizados correctamente."
            });
        }

        // DELETE: api/Tags
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTag(Guid id)
        {
            var tag = await _context.Tags
                .FirstOrDefaultAsync(t => t.Id == id);

            if (tag == null)
            {
                return NotFound(new
                {
                    success = false,
                    data = "Etiqueta no encontrada."
                });
            }

            // Eliminar etiqueta
            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                data = "Etiqueta eliminada correctamente."
            });
        }
    }
}
