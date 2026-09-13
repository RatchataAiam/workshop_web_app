using TodoApi.Dtos;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var todos = new List<TodoGetDto>
{
    new(1, "Learn C#", true),
    new(2, "Learn ASP.NET Core", false),
    new(3, "Build a web API", false)
};

app.MapGet("/api/todos", () => Results.Ok(todos));
app.MapGet("/api/todos/{id}", (int id) =>
{
    var todo = todos.FirstOrDefault(t => t.ID == id);
    return todo != null ? Results.Ok(todo) : Results.NotFound();
});
app.MapPost("/api/todos", (TodoPostDto dto) =>
{
   var newID = todos.Count == 0 ? 1 : todos.Max(t => t.ID) + 1;
   var todo = new TodoGetDto(newID, dto.Title, false);
   todos.Add(todo);
   return Results.Created($"/api/todos/{newID}", todo);
});
app.MapPut("/api/todos/{id}", (int id, TodoPutDto dto) =>
{
    try
    {var index = todos.FindIndex(t => t.ID == id);
    todos[index] = todos[index] with
    {
        Title = dto.Title,
        IsCompleted = dto.IsCompleted
    };
    return Results.Ok(todos[index]);
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
    
});
app.MapDelete("/api/todos/{id}", (int id) =>
{
    try
    {
        var todo = todos.FirstOrDefault(t => t.ID == id);
        if (todo is null) return Results.NotFound();
        todos.Remove(todo);
        return Results.NoContent();
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});
app.Run();