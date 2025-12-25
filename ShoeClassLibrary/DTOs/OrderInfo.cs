using ShoeClassLibrary.Models;
using System.ComponentModel.DataAnnotations;

namespace ShoeClassLibrary.DTOs
{
    /// <summary>
    /// DTO класс со сведениями о заказе
    /// </summary>
    public class OrderInfo
    {
        [Display(Name = "Идентификатор заказа")]
        public int OrderId { get; set; }
        [Display(Name = "Цена всего заказа")]
        public int TotalPrice { get; set; }
        [Display(Name = "Список товаров")]
        public string ShoeArticles { get; set; } = null!;
        [Display(Name = "Дата заказа")]
        public DateOnly? OrderDate { get; set; }

        [Display(Name = "Пользователь")]
        public virtual User User { get; set; } = null!;
    }
}
