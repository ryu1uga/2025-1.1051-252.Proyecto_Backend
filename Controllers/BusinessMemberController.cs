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
    public class BusinessMemberController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BusinessMemberController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/BusinessMembers
        [HttpGet]
        public async Task<IActionResult> GetBusinessMembers()
        {
            var businessMembers = await _context.BusinessMembers
                .OrderBy(bm => bm.Id)
                .Select(bm => new
                {
                    bm.Id,
                    bm.UserId,
                    bm.BusinessId,
                    bm.Role
                })
                .ToListAsync();

            return Ok(new
            {
                success = true,
                data = businessMembers
            });
        }

        // GET: api/BusinessMembers/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBusinessMember(Guid id)
        {
            var businessMember = await _context.BusinessMembers
                .Where(bm => bm.Id == id)
                .Select(bm => new 
                {
                    bm.Id,
                    bm.UserId,
                    bm.BusinessId,
                    bm.Role
                })
                .FirstOrDefaultAsync();

            if (businessMember == null)
                return NotFound(new
                {
                    success = false,
                    data = "Miembro de negocio no encontrado."
                });

            return Ok(new
            {
                success = true,
                data = businessMember
            });
        }

        // POST: api/BusinessMembers
        [HttpPost]
        public async Task<IActionResult> CreateBusinessMember([FromBody] BusinessMemberDTO businessMemberDTO)
        {
            var businessMember = new BusinessMember
            {
                Id = Guid.NewGuid(),
                UserId = businessMemberDTO.UserId,
                BusinessId = businessMemberDTO.BusinessId,
                Role = businessMemberDTO.Role,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.BusinessMembers.Add(businessMember);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                data = "Miembro de negocio creado correctamente."
            });
        }

        // PUT: api/BusinessMembers/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBusinessMember(Guid id, [FromBody] BusinessMemberDTO businessMemberDTO)
        {
            var businessMember = await _context.BusinessMembers.FindAsync(id);

            if (businessMember == null)
            {
                return NotFound(new
                {
                    success = false,
                    data = "Miembro de negocio no encontrado."
                });
            }

            businessMember.Role = businessMemberDTO.Role;
            businessMember.UpdatedAt = DateTime.UtcNow;

            _context.BusinessMembers.Update(businessMember);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                data = "Valores actualizados correctamente."
            });
        }

        // DELETE: api/BusinessMembers
        [HttpDelete]
        public async Task<IActionResult> DeleteBusinessMember(Guid id)
        {
            var businessMember = await _context.BusinessMembers
                .FirstOrDefaultAsync(bm => bm.Id == id);

            if (businessMember == null)
            {
                return NotFound(new
                {
                    success = false,
                    data = "Miembro de negocio no encontrado."
                });
            }

            // Eliminar businessMember
            _context.BusinessMembers.Remove(businessMember);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                data = "Miembro de negocio eliminado correctamente."
            });
        }
    }
}