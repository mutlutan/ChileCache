
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Add DbContext
IServiceCollection serviceCollection = builder.Services.AddDbContext<CacheContext>(static options => options.UseInMemoryDatabase("CacheDatabase"));

// Add CacheService
builder.Services.AddScoped<CacheService>();


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

app.UseAuthorization();

app.MapControllers();


// Minimal API

app.MapPost("/cache", (CacheItem item, CacheService cacheService) =>
{
	cacheService.AddItem(item);
	return Results.Ok();
});

app.MapGet("/cache", (string searchTerm, CacheService cacheService) =>
{
	var items = cacheService.SearchItems(searchTerm);
	return Results.Ok(items);
});

app.MapDelete("/cache/{id}", (int id, CacheService cacheService) =>
{
	cacheService.RemoveItem(id);
	return Results.Ok();
});

app.Run();
