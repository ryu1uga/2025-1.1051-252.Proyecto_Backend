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
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly EmailService _emailService;

        public UsersController(ApplicationDbContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _context.Users
                .OrderBy(u => u.Id)
                .Select(u => new
                {
                    u.Id,
                    u.Name,
                    u.Email
                })
                .ToListAsync();

            return Ok(new
            {
                success = true,
                data = users
            });
        }

        // GET: api/Users/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            var user = await _context.Users
                .Where(u => u.Id == id)
                .Select(u => new 
                {
                    u.Id,
                    u.Name,
                    u.Email
                })
                .FirstOrDefaultAsync();

            if (user == null)
                return NotFound(new
                {
                    success = false,
                    data = "Usuario no encontrado."
                });

            return Ok(new
            {
                success = true,
                data = user
            });
        }

        // POST: api/Users
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserDTO userDTO)
        {
            // Verificar si el email ya está en uso
            if (await _context.Users.AnyAsync(u => u.Email == userDTO.Email))
            {
                return Conflict(new
                {
                    success = true,
                    data = "El email ya se encuentra en uso."
                });
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = userDTO.Name,
                Email = userDTO.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(userDTO.Password),
                UserType = 0,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                data = "Usuario creado correctamente."
            });
        }

        // PUT: api/Users/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UserDTO userDTO)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound(new
                {
                    success = false,
                    data = "Usuario no encontrado."
                });
            }

            // Verificar si el nuevo email ya está en uso por otro usuario
            if (user.Email != userDTO.Email && await _context.Users.AnyAsync(u => u.Email == userDTO.Email))
            {
                return Conflict("El email ya se encuentra en uso.");
            }

            user.Name = userDTO.Name;
            user.Email = userDTO.Email;
            user.Password = BCrypt.Net.BCrypt.HashPassword(userDTO.Password);
            user.UserType = userDTO.UserType;
            user.UpdatedAt = DateTime.UtcNow;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                data = "Valores actualizados correctamente."
            });
        }

        // DELETE: api/Users
        [HttpDelete]
        public async Task<IActionResult> DeleteUser([FromBody] EmailVerificationDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Email))
            {
                return BadRequest(new
                {
                    success = false,
                    data = "El email no puede estar vacío."
                });
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null)
            {
                return NotFound(new
                {
                    success = false,
                    data = "Usuario no encontrado."
                });
            }

            // Eliminar usuario
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                data = "Usuario eliminado correctamente."
            });
        }
    }
}