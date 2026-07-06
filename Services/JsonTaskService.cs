using System.Text.Json;
using TaskTracker.Models;

namespace TaskTracker.Services;

public class JsonTaskService
{
    private const string FilePath = "tasks.json";
    
    public async Task<List<TaskItem>> GetAllTasksAsync()
    {
        if(!File.Exists(FilePath))
        {
            return new List<TaskItem>();
        }

        using var stream = File.OpenRead(FilePath);
        return await JsonSerializer.DeserializeAsync<List<TaskItem>>(stream) ?? new List<TaskItem>();
    }

    private async Task SaveAllTasksAsync(List<TaskItem> tasks)
    {
        using var stream = File.Create(FilePath);
        await JsonSerializer.SerializeAsync(stream, tasks,new JsonSerializerOptions {WriteIndented = true});

    }

    public async Task AddTaskAsync(string text)
    {
        var tasks = await GetAllTasksAsync();
        int nextId = tasks.Count > 0 ? tasks.Max(t => t.Id) + 1 : 1;
        tasks.Add(new TaskItem { Id = nextId, Text = text, IsCompleted = false});
        await SaveAllTasksAsync(tasks);
    }

    public async Task ToggleTaskAsync(int id)
    {
        var tasks = await GetAllTasksAsync();
        var task = tasks.FirstOrDefault(t => t.Id == id);
        if(task != null)
        {
            task.IsCompleted = !task.IsCompleted;
            await SaveAllTasksAsync(tasks);
        }
    }

    public async Task DeleteTaskAsync(int id)
    {
        var tasks = await GetAllTasksAsync();
        tasks.RemoveAll(t => t.Id == id);
        await SaveAllTasksAsync(tasks);
    }

}