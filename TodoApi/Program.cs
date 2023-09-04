using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TodoApiV7;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddDbContext<TodoRepo>(opt => opt.UseInMemoryDatabase("TodoRepo"));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Todo API", Version = "v1" });
});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", async (TodoRepo repo, CancellationToken token, IMapper mapper) =>
    await repo.GetAll(token) is List<TodoItem> results && results.Any()
        ? TypedResults.Ok(mapper.Map<List<TodoItemDto>>(results))
        : Results.NoContent());

app.MapGet("/complete", async (TodoRepo repo, CancellationToken token, IMapper mapper) =>
    await repo.Get(x => x.IsComplete, token) is List<TodoItem> results && results.Any()
        ? TypedResults.Ok(mapper.Map<List<TodoItemDto>>(results))
        : Results.NoContent());

app.MapGet("/outstanding", async (TodoRepo repo, CancellationToken token, IMapper mapper) =>
    await repo.Get(x => !x.IsComplete, token) is List<TodoItem> results && results.Any()
        ? TypedResults.Ok(mapper.Map<List<TodoItemDto>>(results))
        : Results.NoContent());

app.MapGet("/{id}", async (TodoRepo repo, long id, CancellationToken token, IMapper mapper) => 
    await repo.GetById(id, token) is TodoItem result
        ? TypedResults.Ok(mapper.Map<TodoItemDto>(result)) 
        : Results.NoContent());

app.MapGet("/name/{name}", async (TodoRepo repo, string name, CancellationToken token, IMapper mapper) => 
    await repo.Get(x => x.Name == name, token) is List<TodoItem> results && results.Any()
        ? Results.Ok(mapper.Map<List<TodoItemDto>>(results))
        : Results.NotFound());

app.MapPost("/", async (TodoRepo repo, TodoItemDto todo, CancellationToken token, IMapper mapper) => 
    await repo.Create(mapper.Map<TodoItem>(todo), token) is TodoItem result
        ? Results.Created("/{id}", result.Id)
        : Results.UnprocessableEntity());

app.MapPut("/name", async (TodoRepo repo, long id, string name, CancellationToken token, IMapper mapper) =>
    await repo.Update(id, x => x.Name = name, token) is TodoItem result
        ? Results.Ok(mapper.Map<TodoItemDto>(result))
        : Results.NotFound());

app.MapPut("/complete", async (TodoRepo repo, long id, bool isComplete, CancellationToken token, IMapper mapper) =>
    await repo.Update(id, x => x.IsComplete = isComplete, token) is TodoItem result
        ? Results.Ok(mapper.Map<TodoItemDto>(result))
        : Results.NotFound());

app.MapDelete("/{id}", async (TodoRepo repo, long id, CancellationToken token) => 
    await repo.Delete(id, token)
        ? Results.Ok()
        : Results.NotFound());
