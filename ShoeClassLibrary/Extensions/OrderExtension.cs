using ShoeClassLibrary.DTOs;
using ShoeClassLibrary.Models;

namespace ShoeClassLibrary.Extensions
{
    /// <summary>
    /// Методы расширения для преобразования модели заказов в DTO
    /// </summary>
    public static class OrderExtension
    {
        // Преобразует объект заказа в объект DTO
        public static OrderDto? ToDto(this Order order)
            => order is null ? null : new OrderDto
            {
                OrderId = order.OrderId,
                OrderDate = order.OrderDate,
                DeliveryDate = order.DeliveryDate,
                IsFinished = order.IsFinished,
            };

        // Преобразует заказ в расширенную информацию с расчетом стоимости и списком товаров
        public static OrderInfo? ToDtoInfo(this Order order)
        {
            return order is null ? null : new OrderInfo
            {
                OrderId = order.OrderId,
                TotalPrice = order.ShoeOrders.Sum(so => so.Shoe.DiscountPrice * so.Quantity),
                OrderDate = order.OrderDate,
                ShoeArticles = String.Join(", ", order.ShoeOrders.Select(so =>
                    string.Format("[{0}] {1} - {2} шт.", so.Shoe.Article, so.Shoe.Producer.Name, so.Quantity))),
                User = order.User
            };
        }

        // Преобразование массива заказов в краткие информационные модели заказа
        public static IEnumerable<OrderDto?> ToDtos(this IEnumerable<Order> orders)
            => orders.Select(f => f.ToDto());

        public static IEnumerable<OrderInfo?> ToDtoInfos(this IEnumerable<Order> orders)
            => orders.Select(f => f.ToDtoInfo());
    }
}