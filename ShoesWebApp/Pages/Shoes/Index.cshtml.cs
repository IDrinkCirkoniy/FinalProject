using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShoeClassLibrary.Contexts;
using ShoeClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoesWebApp.Pages.Shoes
{
    public class IndexModel : PageModel
    {
        private readonly ShoeClassLibrary.Contexts.ShoeContext _context;
        private Random _random = new();
        private DateOnly _todayDate = DateOnly.FromDateTime(DateTime.Now);
        private DateOnly _nextWeekDate = DateOnly.FromDateTime(DateTime.Now.AddDays(7));
        private int _maxPrice;
        public IndexModel(ShoeClassLibrary.Contexts.ShoeContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public string DescriptionPart { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SortColumn { get; set; }

        [BindProperty(SupportsGet = true)]
        public byte Producer { get; set; }

        [BindProperty(SupportsGet = true)]
        public int MaxPrice
        {
            get => _maxPrice;
            set
            {
                if (value < 0)
                    _maxPrice = 0;
                else
                    _maxPrice = value;
            }
        }

        [BindProperty(SupportsGet = true)]
        public bool IsInStock { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool IsDiscount { get; set; }

        public IList<Shoe> Shoe { get; set; } = default!;

        [BindProperty(SupportsGet = true)]
        public string DiscountColor { get; set; }

        [BindProperty]
        public int SelectedShoeId { get; set; }

        public async Task<IActionResult> OnPostGuestOrderAsync()
        {
            return RedirectToPage("/Login");
        }


        public async Task<IActionResult> OnPostClientOrderAsync()
        {
            var userId = int.Parse(HttpContext.Session.GetString("UserId")); // Получение Id пользователя из сессии

            // Создание нового обьекта заказа
            Order newOrder = new()
            {
                UserId = userId,
                OrderDate = _todayDate,
                DeliveryDate = _nextWeekDate,
                IsFinished = false,
                ReceiveCode = _random.Next(100, 999),
            }; 

            try
            {
                _context.Orders.Add(newOrder); // Добавление нового заказа
                await _context.SaveChangesAsync(); // Сохранение нового заказа

                var order = _context.Entry(newOrder);
                var generatedId = order.Property(o => o.OrderId).CurrentValue; // Получение Id нового заказа

                // Создание новой записи в таблицу соединяющую заказ и товар
                ShoeOrder shoeOrder = new()
                {
                    ShoeId = SelectedShoeId,
                    OrderId = generatedId,
                    Quantity = 1
                }; 

                _context.ShoeOrders.Add(shoeOrder); // Добавление записи
                await _context.SaveChangesAsync(); // Сохранение записи
            }
            catch (Exception ex)
            {
                throw;
            }

            return RedirectToPage("/Index");
        }

        public async Task OnGetAsync()
        {
            ViewData["Producers"] = new SelectList(_context.Producers.Distinct(), "ProducerId", "Name");

            var shoes = _context.Shoes
                .Include(s => s.Category)
                .Include(s => s.Producer)
                .Include(s => s.Vendor)
                .AsQueryable();

            if (!String.IsNullOrEmpty(DescriptionPart))
                shoes = shoes
                    .Where(s => s.Description
                    .Contains(DescriptionPart)); // Поиск по описанию, если строка не является null 

            if (Producer > 0)
                shoes = shoes
                    .Where(s => s.Producer.ProducerId == Producer); // Сортировка по производителю

            if (MaxPrice > 0)
                shoes = shoes
                    .Where(s => s.Price <= MaxPrice); // Сортировка по максимальной цене

            if (IsInStock)
                shoes = shoes.Where(s => s.Quantity > 0); // Сортировка по наличию

            if (IsDiscount)
                shoes = shoes.Where(s => s.Discount > 0); // Сортировка по наличию цены

            shoes = SortColumn switch
            {
                "name" => shoes.OrderBy(s => s.Category.Name),
                "vendor" => shoes.OrderBy(s => s.Vendor.Name),
                "price" => shoes.OrderBy(s => s.Price),
                "price_desc" => shoes.OrderByDescending(s => s.Price),
                _ => shoes
            };

            Shoe = await shoes.ToListAsync();
        }
    }
}
