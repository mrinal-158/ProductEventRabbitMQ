
using Microsoft.EntityFrameworkCore;
using ProductEventRabbitMQ.Data;
using ProductEventRabbitMQ.Interfaces;
using ProductEventRabbitMQ.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ✓ Interface দিয়ে register — Scoped lifetime
builder.Services.AddScoped<IInventoryUpdateService, InventoryUpdateService>();

// ✓ BackgroundService — IServiceScopeFactory দিয়ে Scoped service নেবে
builder.Services.AddHostedService<RabbitOrderConsumer>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.Run();