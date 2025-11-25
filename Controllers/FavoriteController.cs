
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
    public class FavoriteController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FavoriteController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Favorites
        [HttpGet]
        public async Task<IActionResult> GetFavorites()
        {
            var Favorites = await _context.Favorites
                .OrderBy(f => f.Id)
                .Select(f => new
                {
                    f.Id,
                    f.CustomerId,
                    f.ProductId
                })
                .ToListAsync();

            return Ok(new
            {
                success = true,
                data = Favorites
            });
        }

        // GET: api/Favorites/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFavorite(Guid id)
        {
            var favorite = await _context.Favorites
                .Where(f => f.Id == id)
                .Select(f => new 
                {
                    f.Id,
                    f.CustomerId,
                    f.ProductId
                })
                .FirstOrDefaultAsync();

            if (favorite == null)
                return NotFound(new
                {
                    success = false,
                    data = "Producto favorito no encontrado."
                });

            return Ok(new
            {
                success = true,
                data = favorite
            });
        }

        // POST: api/Favorites
        [HttpPost]
        public async Task<IActionResult> CreateFavorite([FromBody] FavoriteDTO favoriteDTO)
        {
            var favorite = new Favorite
            {
                Id = Guid.NewGuid(),
                CustomerId = favoriteDTO.CustomerId,
                ProductId = favoriteDTO.ProductId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Favorites.Add(favorite);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                data = "Producto favorito creado correctamente."
            });
        }
        
        // DELETE: api/Favorites
        [HttpDelete]
        public async Task<IActionResult> DeleteFavorite(Guid id)
        {
            var favorite = await _context.Favorites
                .FirstOrDefaultAsync(f => f.Id == id);

            if (favorite == null)
            {
                return NotFound(new
                {
                    success = false,
                    data = "Producto favorito no encontrado."
                });
            }

            // Eliminar favorite
            _context.Favorites.Remove(favorite);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                data = "Producto favorito eliminado correctamente."
            });
        }
    }
}