using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoeClassLibrary.Contexts;
using ShoeClassLibrary.Models;

namespace ShoesWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoesController : ControllerBase
    {
        private readonly ShoeContext _context;

        public ShoesController(ShoeContext context)
        {
            _context = context;
        }

        // GET: api/Shoes
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Shoe>>> GetShoes()
        {
            return await _context.Shoes.ToListAsync(); // Метод Get для получения данных о товарах из базы данных
        }

        // GET: api/Shoes/5
        [AllowAnonymous]
        [HttpGet("{article}")] // Метод Get для получение товара по артикулу
        public async Task<ActionResult<Shoe>> GetShoe(string article)
        {
            var shoe = await _context.Shoes.FirstOrDefaultAsync(s => s.Article == article);

            if (shoe == null)
                return NotFound();

            return shoe;
        }

        // PUT: api/Shoes/5
        [Authorize(Roles = "admin, manager")] // Атрибут дающий доступ только пользователям с ролью admin, manager
        [HttpPut("{shoeId}")] // Метод Put для изменение товара по id
        public async Task<IActionResult> PutShoe(int shoeId, Shoe shoe)
        {
            if (shoeId != shoe.ShoeId)
                return BadRequest();

            _context.Entry(shoe).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShoeExists(shoeId))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // POST: api/Shoes
        [Authorize(Roles = "admin, manager")] // Атрибут дающий доступ только пользователям с ролью admin, manager
        [HttpPost] // Метод Post для вставки новых моделей обуви
        public async Task<ActionResult<Shoe>> PostShoe(Shoe shoe)
        {
            _context.Shoes.Add(shoe);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetShoe", new { shoeId = shoe.ShoeId }, shoe);
        }

        // DELETE: api/Shoes/5
        [Authorize(Roles = "admin, manager")] // Атрибут дающий доступ только пользователям с ролью admin, manager
        [HttpDelete("{shoeId}")] // Метод Delete для удаления обуви по Id
        public async Task<IActionResult> DeleteShoe(int shoeId)
        {
            var shoe = await _context.Shoes.FindAsync(shoeId);
            if (shoe == null)
                return NotFound();

            _context.Shoes.Remove(shoe);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ShoeExists(int shoeId)
        {
            return _context.Shoes.Any(e => e.ShoeId == shoeId);
        }
    }
}
