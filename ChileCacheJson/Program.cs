using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

var cache = new ConcurrentDictionary<Guid, JsonDocument>();

app.MapPost("/cache", ([FromBody] JsonDocument data) =>
{
	var id = Guid.NewGuid();
	cache[id] = data;
	return Results.Ok(new { Id = id });
}).WithName("AddToCache").WithOpenApi();

app.MapGet("/cache/{id}", (Guid id) =>
{
	if (cache.TryGetValue(id, out var row))
	{
		return Results.Ok(JsonSerializer.Serialize(row));
	}
	return Results.NotFound();
}).WithName("GetFromCache").WithOpenApi();

app.MapPost("/cache/search", ([FromBody] dynamic payload) =>
{
	Guid id = payload.id;
	IDictionary<string, string> searchParams = payload.query.ToObject<IDictionary<string, string>>();

	if (!cache.TryGetValue(id, out var row))
	{
		return Results.NotFound();
	}

	var jsonElement = row.RootElement;
	var matches = searchParams.All(param =>
		jsonElement.TryGetProperty(param.Key, out var value) &&
		value.ToString().Contains(param.Value, StringComparison.OrdinalIgnoreCase));

	return matches ? Results.Ok(JsonSerializer.Serialize(row)) : Results.NotFound();
}).WithName("SearchCache").WithOpenApi();

app.MapDelete("/cache/{id}", (Guid id) =>
{
	if (cache.TryRemove(id, out _))
	{
		return Results.Ok();
	}
	return Results.NotFound();
}).WithName("RemoveFromCache").WithOpenApi();

app.Run();
