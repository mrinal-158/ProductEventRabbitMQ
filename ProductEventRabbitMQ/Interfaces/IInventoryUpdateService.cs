using ProductEventRabbitMQ.Model;

namespace ProductEventRabbitMQ.Interfaces
{
    public interface IInventoryUpdateService
    {
        Task UpdateInventoryAsync(Product product);
    }
}
