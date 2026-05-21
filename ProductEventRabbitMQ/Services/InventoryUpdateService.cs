using ProductEventRabbitMQ.Data;
using ProductEventRabbitMQ.Interfaces;
using ProductEventRabbitMQ.Model;

namespace ProductEventRabbitMQ.Services
{
    public class InventoryUpdateService : IInventoryUpdateService
    {
        private readonly ILogger<InventoryUpdateService> _logger;
        private readonly AppDbContext _context;
        public InventoryUpdateService(ILogger<InventoryUpdateService> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task UpdateInventoryAsync(Product product)
        {
            var productInDb = await _context.Products.FindAsync(product.Id);
            if (productInDb is null)
                _logger.LogWarning("Product with Id {ProductId} not found in database.", product.Id);
            if (productInDb.Stock <= 0)
                _logger.LogWarning("Product with Id {ProductId} is out of stock.", product.Id);
            productInDb.Stock -= 1;
            await _context.SaveChangesAsync();
            _logger.LogInformation("Received order for product {ProductId}. Updated stock: {Stock}", product.Id, productInDb.Stock);
        }

    }
}
