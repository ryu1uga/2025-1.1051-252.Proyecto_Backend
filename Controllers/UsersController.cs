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

        private async Task<bool> ValidateToken(Guid userId, int? token)
        {
            var user = await _context.Users.FindAsync(userId);
            return user != null && user.Token == token;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<IActionResult> GetUsers([FromHeader(Name = "UserId")] Guid userId, [FromHeader(Name = "Token")] int? token)
        {
            if (!await ValidateToken(userId, token))
            {
                return Unauthorized(new
                {
                    success = false,
                    data = "El token ingresado no es válido."
                });
            }
            
            var users = await _context.Users
                .Include(u => u.Profiles)
                .ThenInclude(p => p.Avatar)
                .OrderBy(u => u.Id)
                .Select(u => new
                {
                    u.Id,
                    u.Name,
                    u.Email,
                    Profiles = u.Profiles.Select(p => new
                    {
                        Id = p.Id,
                        AvatarId = p.AvatarId,
                        AvatarUrl = p.Avatar.AvatarUrl,
                        Username = p.Username,
                        Gender = p.Gender,
                        BirthDate = p.BirthDate
                    }).ToList()
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
        public async Task<IActionResult> GetUser(Guid id, [FromHeader(Name = "Token")] int? token)
        {
            if (!await ValidateToken(id, token))
            {
                return Unauthorized(new
                {
                    success = false,
                    data = "El token ingresado no es válido."
                });
            }
            
            var user = await _context.Users
                .Include(u => u.Profiles)
                    .ThenInclude(p => p.Avatar)
                .Where(u => u.Id == id)
                .Select(u => new 
                {
                    u.Id,
                    u.Name,
                    u.Email,
                    Profiles = u.Profiles.Select(p => new
                    {
                        Id = p.Id,
                        AvatarId = p.AvatarId,
                        AvatarUrl = p.Avatar.AvatarUrl,
                        Username = p.Username,
                        Gender = p.Gender,
                        BirthDate = p.BirthDate
                    }).ToList()
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

        // GET: api/Users/{id}/profiles
        [HttpGet("{id}/profiles")]
        public async Task<IActionResult> GetUserProfiles(Guid id, [FromHeader(Name = "Token")] int? token)
        {
            if (!await ValidateToken(id, token))
            {
                return Unauthorized(new
                {
                    success = false,
                    data = "El token ingresado no es válido."
                });
            }
            
            var user = await _context.Users
                .Include(u => u.Profiles)
                    .ThenInclude(p => p.Avatar)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                return NotFound(new
                {
                    success = false,
                    data = "Usuario no encontrado."
                });

            if (user.Profiles == null || user.Profiles.Count == 0)
                return Ok(new
                {
                    success = true,
                    data = new List<ProfileDTO>()
                });

            var profilesDTO = user.Profiles
            .OrderBy(profile => profile.Id)
            .Select(profile => new
            {
                Id = profile.Id,
                AvatarId = profile.AvatarId,
                AvatarUrl = profile.Avatar.AvatarUrl,
                Username = profile.Username,
                Gender = profile.Gender,
                BirthDate = profile.BirthDate
            }).ToList();

            return Ok(new
            {
                success = true,
                data = profilesDTO
            });
        }

        // GET: api/Users/count
        [HttpGet("count")]
        public async Task<IActionResult> GetUsersCount([FromHeader(Name = "UserId")] Guid userId, [FromHeader(Name = "Token")] int? token)
        {
            if (!await ValidateToken(userId, token))
            {
                return Unauthorized(new
                {
                    success = false,
                    data = "El token ingresado no es válido."
                });
            }

            int count = await _context.Users.CountAsync();

            return Ok(new
            {
                success = true,
                data = count
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
                UpdatedAt = DateTime.UtcNow,
                VerificationCode = null,
                CodeRegisteredDate = null,
                CodeExpirationDate = null,
                Token = 0
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
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UserDTO userDTO, [FromHeader(Name = "Token")] int? token)
        {
            if (!await ValidateToken(id, token))
            {
                return Unauthorized(new
                {
                    success = false,
                    data = "El token ingresado no es válido."
                });
            }
            
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
            user.UpdatedAt = DateTime.UtcNow;
            user.VerificationCode = null;
            user.CodeRegisteredDate = null;
            user.CodeExpirationDate = null;
            user.Token = null;

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
                .Include(u => u.Profiles)
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null)
            {
                return NotFound(new
                {
                    success = false,
                    data = "Usuario no encontrado."
                });
            }

            // Validar código de verificación
            if (user.VerificationCode != dto.Code || user.CodeExpirationDate <= DateTime.UtcNow)
            {
                return Unauthorized(new
                {
                    success = false,
                    data = "El código ingresado no es válido o ha expirado."
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

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> LoginUser(LoginRequestDTO loginRequestDTO)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == loginRequestDTO.Email);

            if (user == null)
            {
                return Unauthorized(new
                {
                    success = false,
                    data = "El email ingresado no existe."
                });
            }

            if (!BCrypt.Net.BCrypt.Verify(loginRequestDTO.Password, user.Password))
            {
                return Unauthorized(new
                {
                    success = false,
                    data = "Contraseña incorrecta."
                });
            }

            int? newToken;
            if (user.Token == 10000)
            {
                newToken = 10000;
            }
            else
            {
                var random = new Random();
                do
                {
                    newToken = random.Next(1000, 10000);
                } while (await _context.Users.AnyAsync(u => u.Token == newToken));
            }
            user.Token = newToken;


            

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            // Crea el DTO de usuario con los datos necesarios para la respuesta
            var userDTO = new
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Token = newToken
            };

            return Ok(new
            {
                success = true,
                data = userDTO
            });
        }

        // POST: api/Users/logout/{id}
        [HttpPost("logout/{id}")]
        public async Task<IActionResult> LogoutUser(Guid id, [FromHeader(Name = "Token")] int? token)
        {
            if (!await ValidateToken(id, token))
            {
                return Unauthorized(new
                {
                    success = false,
                    data = "El token ingresado no es válido."
                });
            }
            
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound(new
                {
                    success = false,
                    data = "Usuario no encontrado."
                });
            }

            user.Token = null;
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                data = "Cierre de sesión realizado exitosamente."
            });
        }

        // POST: api/Users/verifyemail
        [HttpPost("verifyemail")]
        public async Task<IActionResult> VerifyEmail([FromBody] EmailDTO emailDTO)
        {
            if (string.IsNullOrWhiteSpace(emailDTO.Email))
            {
                return BadRequest(new
                {
                    success = false,
                    data = "El campo de email no puede estar vacío."
                });
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == emailDTO.Email);
            if (user == null)
            {
                return NotFound(new { success = false, data = "El email ingresado no existe." });
            }

            var random = new Random();
            user.VerificationCode = random.Next(1000, 10000); // código de 4 cifras
            user.CodeRegisteredDate = DateTime.UtcNow;
            user.CodeExpirationDate = DateTime.UtcNow.AddDays(1);

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            // Enviar el código por email
            var subject = "Código de verificación";
            var body = $"<p>Hola {user.Name},</p><p>Tu código de verificación es: <strong>{user.VerificationCode}</strong></p><p>Este código expirará en 24 horas.</p>";

            await _emailService.SendEmailAsync(user.Email, subject, body);

            return Ok(new
            {
                success = true,
                data = new
                {
                    message = "Código de verificación enviado al email."
                }
            });
        }

        [HttpPost("set-password")]
        public async Task<IActionResult> SetNewPassword([FromBody] ResetPasswordDTO dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null)
            {
                return NotFound(new { success = false, data = "Usuario no encontrado." });
            }

            if (user.VerificationCode != dto.Code || user.CodeExpirationDate <= DateTime.UtcNow)
            {
                return Unauthorized(new { success = false, data = "El código ingresado no es válido o ha expirado." });
            }

            if (dto.Password != dto.ConfirmPassword)
            {
                return Unauthorized(new { success = false, data = "Las contraseñas no coinciden." });
            }

            user.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            user.VerificationCode = null;
            user.CodeRegisteredDate = null;
            user.CodeExpirationDate = null;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, data = "Contraseña actualizada correctamente." });
        }
    }
}