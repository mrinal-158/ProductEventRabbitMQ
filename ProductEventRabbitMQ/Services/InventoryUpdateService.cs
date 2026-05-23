using Microsoft.EntityFrameworkCore;
using ProductEventRabbitMQ.Data;
using ProductEventRabbitMQ.Interfaces;
using ProductEventRabbitMQ.Model;
using ProductEventRabbitMQ.Models;

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

        public async Task UpdateInventoryAsync(Order order)
        {
            if (order == null)
            {
                _logger.LogWarning("Order is null");
                return;
            }
            var productInDb = await _context.Products.FirstOrDefaultAsync(p => p.Id == order.ProductId);
            if (productInDb is null)
            {
                _logger.LogWarning("Product with Id {ProductId} not found in database.", productInDb.Id);
                return;
            }
            else if (productInDb.Stock <= 0)
            {
                _logger.LogWarning("Product with Id {ProductId} is out of stock.", productInDb.Id);
                return;
            }
            else
            {
                productInDb.Stock -= 1;
                await _context.SaveChangesAsync();
                _logger.LogInformation("Received order for product {ProductId}. Updated stock: {Stock}", productInDb.Id, productInDb.Stock);
                return;
            }
        }

    }
}
