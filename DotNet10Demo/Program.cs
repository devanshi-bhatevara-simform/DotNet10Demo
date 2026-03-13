using DotNet10Demo.Data;
using DotNet10Demo.Filters;
using DotNet10Demo.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlite("Data Source=products.db"));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
    db.Products.AddRange(
        new Product { Id = 1, Name = "Active Product",           IsDeleted = false, IsActive = true,  Details = new() { Description = "A great product",    Tags = ["sale", "new"],    Embedding = [0.9f, 0.1f, 0.2f] } },
        new Product { Id = 2, Name = "Inactive Product",         IsDeleted = false, IsActive = false, Details = new() { Description = "Coming soon",         Tags = ["upcoming"],       Embedding = [0.1f, 0.8f, 0.3f] } },
        new Product { Id = 3, Name = "Deleted Product",          IsDeleted = true,  IsActive = true,  Details = new() { Description = "No longer available", Tags = ["discontinued"],   Embedding = [0.2f, 0.3f, 0.9f] } },
        new Product { Id = 4, Name = "Deleted+Inactive Product", IsDeleted = true,  IsActive = false, Details = new() { Description = "Old product",         Tags = ["old", "hidden"],  Embedding = [0.5f, 0.5f, 0.5f] } }
    );
    db.SaveChanges();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

// Minimal API - endpoint filter for logging
app.MapGet("/hello", (string? name) => $"Hello, {name ?? "World"}!")
   .AddEndpointFilter<LoggingFilter>();

// Named Query Filters - returns only active, non-deleted products
// ProductDetails (Description + Tags) is stored as a native JSON column
app.MapGet("/products", async (AppDbContext db) =>
    await db.Products.ToListAsync());

app.Run();
