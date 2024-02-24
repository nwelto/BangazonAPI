using BangazonAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;


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

app.MapGet("/api/products", async (BangazonAPIDbContext db) =>
    await db.Products.Include(p => p.Category).ToListAsync());

app.MapGet("/api/products/{id}", async (BangazonAPIDbContext db, int id) =>
{
    var product = await db.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);
    return product != null ? Results.Ok(product) : Results.NotFound();
});

app.MapPost("/api/products", async (BangazonAPIDbContext db, Product product) =>
{
    db.Products.Add(product);
    await db.SaveChangesAsync();
    return Results.Created($"/api/products/{product.Id}", product);
});

app.MapPut("/api/products/{id}", async (BangazonAPIDbContext db, int id, Product updatedProduct) =>
{
    var product = await db.Products.FindAsync(id);

    if (product == null) return Results.NotFound();

    product.Name = updatedProduct.Name;
    product.Description = updatedProduct.Description;
    product.Price = updatedProduct.Price;
    product.Quantity = updatedProduct.Quantity;
    product.CategoryId = updatedProduct.CategoryId;

    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/api/products/{id}", async (BangazonAPIDbContext db, int id) =>
{
    var product = await db.Products.FindAsync(id);
    if (product == null) return Results.NotFound();

    db.Products.Remove(product);
    await db.SaveChangesAsync();
    return Results.NoContent();
});




app.Run();

