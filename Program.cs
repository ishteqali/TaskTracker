using TaskTracker.Services;
using TaskTracker.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<JsonTaskService>();

var app = builder.Build();

app.MapGet("/api/tasks", async (JsonTaskService service) => Results.Ok(await service.GetAllTasksAsync()));
app.MapPost("/api/tasks", async (TaskItem input, JsonTaskService service) =>
{
    // changes- adding input validation at server side
    if (input == null || string.IsNullOrWhiteSpace(input.Text))
    {
        return Results.BadRequest(new { message = "Task text cannot be empty." });
    }


    await service.AddTaskAsync(input.Text);
    return Results.Created();
});
app.MapPut("/api/tasks/{id:int}", async (int id, JsonTaskService service) =>
{
    await service.ToggleTaskAsync(id);
    return Results.NoContent();
});
app.MapDelete("/api/tasks/{id:int}", async (int id, JsonTaskService service) =>
{
    await service.DeleteTaskAsync(id);
    return Results.NoContent();
});

app.MapGet("/", async () =>
{
    var html = await File.ReadAllTextAsync("index.html");
    return Results.Content(html, "text/html");
});

app.Run();
