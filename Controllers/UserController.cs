using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Loop.Data;
using Loop.DTOs.Common;
using Loop.Models.Common;
using Loop.Services;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using FirebaseAdmin.Auth;

namespace Loop.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly EmailService _emailService;
        private readonly ILogger<UserController> _logger;

        public UserController(ApplicationDbContext context, IConfiguration configuration, EmailService emailService, ILogger<UserController> logger)
        {
            _context = context;
            _configuration = configuration;
            _emailService = emailService;
            _logger = logger;
        }

        // GET: api/Users
        [Authorize]
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
        [Authorize]
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

        [Authorize]
        [HttpGet("audit-logs")]
        public IActionResult GetAuditLogs()
        {
            try
            {
                var logDirectory = Path.Combine(AppContext.BaseDirectory, "Logs");
                var logFile = Directory.GetFiles(logDirectory, "login-audit-*.log")
                                    .OrderByDescending(f => f)
                                    .FirstOrDefault();

                if (logFile == null)
                {
                    return NotFound(new { success = false, data = "No se encontraron logs de auditoría." });
                }

                var logContent = System.IO.File.ReadAllText(logFile);

                return Ok(new
                {
                    success = true,
                    data = logContent
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al leer los logs de auditoría.");
                return StatusCode(500, new { success = false, data = "Error al leer los logs." });
            }
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
                return Unauthorized(new { success = false, data = "Usuario no encontrado." });

            user.RefreshToken = null;
            user.TokenExpirationDate = null;

            await _context.SaveChangesAsync();

            return Ok(new { success = true, data = "Sesión cerrada correctamente." });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRequestDTO tokenRequest)
        {
            if (tokenRequest is null)
                return BadRequest("Solicitud inválida.");

            var principal = GetPrincipalFromExpiredToken(tokenRequest.Token);
            if (principal == null)
                return BadRequest("Token inválido.");

            var email = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            // ⬇️ Verificación con BCrypt si guardas el refresh token hasheado
            if (user == null ||
                !BCrypt.Net.BCrypt.Verify(tokenRequest.RefreshToken, user.RefreshToken) ||
                user.TokenExpirationDate <= DateTime.UtcNow)
            {
                return Unauthorized("Refresh token inválido o expirado.");
            }

            var newJwtToken = GenerateJwtToken(user);
            var newRefreshToken = GenerateRefreshToken();

            // 🔒 Hasheas el nuevo refresh token antes de guardarlo
            user.RefreshToken = BCrypt.Net.BCrypt.HashPassword(newRefreshToken);
            user.TokenExpirationDate = DateTime.UtcNow.AddDays(7);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                data = new
                {
                    token = newJwtToken,
                    refreshToken = newRefreshToken
                }
            });
        }

        // POST: api/Users/firebase-auth
        [HttpPost("firebase-auth")]
        public async Task<IActionResult> AuthenticateWithFirebase([FromBody] FirebaseTokenDTO tokenDTO)
        {
            try
            {
                // 1. Verificar el token de Firebase (delegado al SDK)
                FirebaseToken decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(tokenDTO.IdToken);

                string email = decodedToken.Claims.ContainsKey("email") ? decodedToken.Claims["email"].ToString() : null;
                string uid = decodedToken.Uid;
                string name = decodedToken.Claims.ContainsKey("name") ? decodedToken.Claims["name"].ToString() : "Usuario Firebase";

                if (string.IsNullOrEmpty(email))
                {
                    return BadRequest(new { success = false, message = "Token de Firebase no contiene email válido." });
                }

                // 2. Buscar o crear el usuario en tu DB local (User)
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

                if (user == null)
                {
                    // Usuario nuevo: crear registro en la DB local
                    user = new User
                    {
                        Id = Guid.NewGuid(),
                        Name = name,
                        Email = email,
                        UserType = 0,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    _context.Users.Add(user);
                }
                else
                {
                    // Usuario existente: solo actualizar info/último login
                    user.Name = name; 
                    user.UpdatedAt = DateTime.UtcNow;
                    _context.Users.Update(user);
                }

                await _context.SaveChangesAsync();

                // 3. Generar tu JWT interno (si lo mantienes para comunicación entre microservicios)
                var internalJwt = GenerateJwtToken(user);

                return Ok(new
                {
                    success = true,
                    data = new
                    {
                        message = "Autenticación exitosa vía Firebase.",
                        token = internalJwt,
                        userId = user.Id
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error durante la autenticación de Firebase.");
                return Unauthorized(new { success = false, message = "Token de Firebase inválido o expirado." });
            }
        }

        // PUT: api/Users/{id}
        [Authorize]
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
        [Authorize]
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

        // 🔹 Método Privado para Generar JWT
        private string GenerateJwtToken(User user)
        {
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.Email)
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.ASCII.GetBytes(_configuration["Jwt:Key"])
                ),
                ValidateLifetime = false // Ignora expiración
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
                if (securityToken is not JwtSecurityToken jwt ||
                    !jwt.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                    return null;

                return principal;
            }
            catch
            {
                return null;
            }
        }
    }
}