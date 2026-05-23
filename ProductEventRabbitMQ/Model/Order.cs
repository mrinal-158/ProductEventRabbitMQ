namespace ProductEventRabbitMQ.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;
        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Unpaid;
    }
}

public enum OrderStatus
{
    Pending,
    Processing,
    Completed,
    Cancelled
}

public enum PaymentStatus
{
    Unpaid,
    Paid,
    Failed,
    Refunded
}