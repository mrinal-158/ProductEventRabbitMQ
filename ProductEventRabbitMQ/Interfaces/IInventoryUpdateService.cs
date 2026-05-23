using ProductEventRabbitMQ.Model;
using ProductEventRabbitMQ.Models;

namespace ProductEventRabbitMQ.Interfaces
{
    public interface IInventoryUpdateService
    {
        Task UpdateInventoryAsync(Order order);
    }
}
