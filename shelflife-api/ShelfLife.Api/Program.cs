// using ShelfLife.API; // We will create this namespace
using Microsoft.AspNetCore.OpenApi; // For documentation
using Microsoft.EntityFrameworkCore;
using ShelfLife.Api.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(); // <--- This enables Controllers
builder.Services.AddOpenApi(); // API Documentation

// Configure Entity Framework with PostgreSQL
builder.Services.AddDbContext<ShelfLifeDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); // Exposes the documentation endpoint
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers(); // <--- This maps the routes to your Controller classes

app.Run();