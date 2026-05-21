using ProductEventRabbitMQ.Data;
using ProductEventRabbitMQ.Interfaces;
using ProductEventRabbitMQ.Model;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace ProductEventRabbitMQ.Services
{
    public class RabbitOrderConsumer : BackgroundService
    {
        private const string ExchangeName = "order.exchange";
        private const string QueueName = "inventory.queue";

        private readonly ILogger<RabbitOrderConsumer> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public RabbitOrderConsumer(ILogger<RabbitOrderConsumer> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }
        protected override async Task ExecuteAsync(CancellationToken ct)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };

            var connection = await factory.CreateConnectionAsync(ct);
            var channel = await connection.CreateChannelAsync(cancellationToken: ct);

            //Exchange declare 
            await channel.ExchangeDeclareAsync(
                exchange: ExchangeName,
                type: ExchangeType.Fanout,
                durable: true
            );

            //Inventory Queue declare
            await channel.QueueDeclareAsync(
                queue: QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false
            );

            //Binding Queue to Exchange
            await channel.QueueBindAsync(
                queue: QueueName,
                exchange: ExchangeName,
                routingKey: string.Empty
            );

            //one for inventory queue
            await channel.BasicQosAsync(
                prefetchSize: 0,
                prefetchCount: 1,
                global: false
            );

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += async (sender, eventArgs) =>
            {
                var body = eventArgs.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var product = System.Text.Json.JsonSerializer.Deserialize<Product>(message);
                // Create new scope for each message
                using var scope = _scopeFactory.CreateScope();
                var inventoryService = scope.ServiceProvider.GetRequiredService<InventoryUpdateService>();
                await inventoryService.UpdateInventoryAsync(product);
            };

            await channel.BasicConsumeAsync(
                queue: QueueName,
                autoAck: false,
                consumer: consumer
            );
            await Task.Delay(Timeout.Infinite, ct);
        }
    }
}
