var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton(new TodoStore("todos.json"));

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapGet("/api/todos", (TodoStore store) => store.GetAll());

app.MapPost("/api/todos", (TodoStore store, NewTodo body) =>
{
    if (string.IsNullOrWhiteSpace(body.Title))
        return Results.BadRequest("Title is required.");
    store.Add(body.Title.Trim());
    return Results.Ok();
});

app.MapPut("/api/todos/{id}/toggle", (TodoStore store, int id) =>
    store.Toggle(id) ? Results.NoContent() : Results.NotFound());

app.MapDelete("/api/todos/{id}", (TodoStore store, int id) =>
    store.Delete(id) ? Results.NoContent() : Results.NotFound());

app.Run();

record NewTodo(string Title);