using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ShoeClassLibrary.DTOs;
using ShoeClassLibrary.Extensions;
using ShoeClassLibrary.Models;

namespace ShoesWebApp.Pages.Orders
{
    public class IndexModel : PageModel
    {
        private readonly ShoeClassLibrary.Contexts.ShoeContext _context;
        public IndexModel(ShoeClassLibrary.Contexts.ShoeContext context)
        {
            _context = context;
        }

        public IList<OrderInfo> OrderInfos { get; set; } = default!;

        public async Task OnGetClientAsync()
        {
            var userOrders = _context.Orders
                .Include(o => o.User)
                .Include(o => o.ShoeOrders)
                    .ThenInclude(s => s.Shoe)
                        .ThenInclude(s => s.Producer)
                .Where(o => o.User.UserId == int.Parse(HttpContext.Session.GetString("UserId")))
                .AsQueryable();

            IEnumerable<Order?> orderInfos = await userOrders.ToListAsync();

            OrderInfos = orderInfos.ToDtoInfos().ToList();
        }

        public async Task OnGetManagerAsync()
        {
            var orders = _context.Orders
                .Include(o => o.User)
                .Include(o => o.ShoeOrders)
                    .ThenInclude(s => s.Shoe)
                        .ThenInclude(s => s.Producer)
                .AsQueryable();

            IEnumerable<Order?> orderInfos = await orders.ToListAsync();

            OrderInfos = orders.ToDtoInfos().ToList();
        }
    }
}
