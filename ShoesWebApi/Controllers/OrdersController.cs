using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoeClassLibrary.Contexts;
using ShoeClassLibrary.DTOs;
using ShoeClassLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoeClassLibrary.Extensions;

namespace ShoesWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ShoeContext _context;

        private DateOnly actualDate = DateOnly.FromDateTime(DateTime.Now);

        // GET: api/Orders
        [Authorize] // Атрибут дающий доступ авторизованным пользователям
        [HttpGet("{login}")] // Метод Get для получения заказов по логину
        public async Task<ActionResult<IEnumerable<OrderDto?>>> GetOrdersByLogin(string login)
        {
            var orders = await _context.Orders
                .Include(o => o.User)
                .Where(o => o.User.Login == login)
                .ToListAsync() ?? null!;

            return orders is null ?
                NotFound() :
                Ok(orders.ToDtos());
        }

        // POST: api/Orders
        [Authorize(Roles = "admin, manager")] // Атрибуд дающий доступ только пользователям с ролью admin, manager
        [HttpPut("{id}/delivered")] // Метод Put для изменения свойств и даты заказа по Id
        public async Task<ActionResult<OrderDto>> PutOrderDate(int id, [FromQuery] DateOnly? deliveryDate = null, [FromQuery] bool isFinished = false)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order is null)
                return NotFound();

            if (deliveryDate is null)
                order.DeliveryDate = actualDate;
            else
                order.DeliveryDate = deliveryDate;

            order.IsFinished = isFinished;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }

            return order.ToDto();
        }
    }
}
