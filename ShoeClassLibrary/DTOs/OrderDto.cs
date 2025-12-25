namespace ShoeClassLibrary.DTOs
{
    /// <summary>
    /// Dto класс заказа для отображения кратких сведений из истории заказов
    /// </summary>
    public class OrderDto
    {
        public int OrderId { get; set; }
        public DateOnly? OrderDate { get; set; }
        public DateOnly? DeliveryDate { get; set; }
        public bool IsFinished { get; set; }
    }
}
