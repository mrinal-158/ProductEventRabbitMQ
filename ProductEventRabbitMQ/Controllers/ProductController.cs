using Microsoft.AspNetCore.Mvc;
using ProductEventRabbitMQ.Data;
using ProductEventRabbitMQ.Model;

namespace ProductEventRabbitMQ.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ProductController(AppDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return Ok(new
            {
                message = "Product created successfully",
                product = product
            });
        }
    }
}
