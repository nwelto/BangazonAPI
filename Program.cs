using BangazonAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using BangazonAPI.DTOs;


var builder = WebApplication.CreateBuilder(args);

// allows passing datetimes without time zone data 
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// allows our api endpoints to access the database through Entity Framework Core
builder.Services.AddNpgsql<BangazonAPIDbContext>(builder.Configuration["BangazonAPIDbConnectionString"]);

// Set the JSON serializer options
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/api/products", (BangazonAPIDbContext db) =>
    db.Products.Include(p => p.Category).ToList());

app.MapGet("/api/products/{id}", (BangazonAPIDbContext db, int id) =>
{
    var product = db.Products.Include(p => p.Category).FirstOrDefault(p => p.Id == id);
    return product != null ? Results.Ok(product) : Results.NotFound();
});

app.MapPost("/api/products", (BangazonAPIDbContext db, Product product) =>
{
    db.Products.Add(product);
    db.SaveChanges();
    return Results.Created($"/api/products/{product.Id}", product);
});

app.MapPut("/api/products/{id}", (BangazonAPIDbContext db, int id, Product updatedProduct) =>
{
    var product = db.Products.Find(id);

    if (product == null) return Results.NotFound();

    product.Name = updatedProduct.Name;
    product.Description = updatedProduct.Description;
    product.Price = updatedProduct.Price;
    product.Quantity = updatedProduct.Quantity;
    product.CategoryId = updatedProduct.CategoryId;

    db.SaveChanges();
    return Results.NoContent();
});

app.MapDelete("/api/products/{id}", (BangazonAPIDbContext db, int id) =>
{
    var product = db.Products.Find(id);
    if (product == null) return Results.NotFound();

    db.Products.Remove(product);
    db.SaveChanges();
    return Results.NoContent();
});


app.MapGet("/api/orders", (BangazonAPIDbContext db) =>
    db.Orders.Include(o => o.User).ToList());

app.MapGet("/api/orders/{id}", (BangazonAPIDbContext db, int id) =>
{
    var order = db.Orders.Include(o => o.User).FirstOrDefault(o => o.Id == id);
    return order != null ? Results.Ok(order) : Results.NotFound();
});

app.MapPost("/api/orders", (BangazonAPIDbContext db, OrderCreateDto orderDto) =>
{
    var order = new Order
    {
        UserId = orderDto.UserId,
        IsOpen = orderDto.IsOpen,
        Created = DateTime.UtcNow, 
        OrderProducts = new List<OrderProduct>()
    };

    foreach (var productId in orderDto.ProductIds)
    {
        var productExists = db.Products.Any(p => p.Id == productId);
        if (!productExists)
        {
            return Results.NotFound($"Product with ID {productId} not found.");
        }

        order.OrderProducts.Add(new OrderProduct
        {
            ProductId = productId

        });
    }

    db.Orders.Add(order);
    db.SaveChanges();

    return Results.Created($"/api/orders/{order.Id}", order);
});

app.MapPut("/api/orders/{id}", (BangazonAPIDbContext db, int id, Order updatedOrder) =>
{
    var order = db.Orders.Find(id);

    if (order == null) return Results.NotFound();

    order.UserId = updatedOrder.UserId;
    order.IsOpen = updatedOrder.IsOpen;

    db.SaveChanges();

    return Results.NoContent();
});

app.MapDelete("/api/orders/{id}", (BangazonAPIDbContext db, int id) =>
{
    var order = db.Orders.Find(id);
    if (order == null) return Results.NotFound();

    db.Orders.Remove(order);
    db.SaveChanges();
    return Results.NoContent();
});


app.MapGet("/api/categories", (BangazonAPIDbContext db) =>
    db.Categories.Select(c => new CategoryDto { Id = c.Id, Name = c.Name }).ToList());



app.MapPost("/api/categories", (Category category, BangazonAPIDbContext db) =>
{
    db.Categories.Add(category);
    db.SaveChanges();
    return Results.Created($"/api/categories/{category.Id}", category);
});




app.MapGet("/api/categories/{id}", (BangazonAPIDbContext db, int id) =>
{
    var category = db.Categories
        .Where(c => c.Id == id)
        .Select(c => new CategoryDto { Id = c.Id, Name = c.Name })
        .FirstOrDefault();

    return category != null ? Results.Ok(category) : Results.NotFound();
});


app.MapPut("/api/categories/{id}", (BangazonAPIDbContext db, int id, Category updatedCategory) =>
{
    var existingCategory = db.Categories.Find(id);
    if (existingCategory == null)
        return Results.NotFound();

    existingCategory.Name = updatedCategory.Name;

    db.SaveChanges();
    return Results.Ok(existingCategory);
});


app.MapDelete("/api/categories/{id}", (BangazonAPIDbContext db, int id) =>
{
    var categoryToDelete = db.Categories.Find(id);
    if (categoryToDelete == null)
        return Results.NotFound();

    db.Categories.Remove(categoryToDelete);
    db.SaveChanges();

    return Results.NoContent();
});


app.Run();

