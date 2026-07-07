namespace TaskTracker.Models;

public class TaskItem
{
    public int Id {get; set; }
    public string Text {get; set; } = string.Empty;
    public bool IsCompleted {get; set;}

    // changes- using UtcNow for storing date on server. Javascript handles date for local system in frontend.
    public DateTime CreatedAt {get; set;} = DateTime.UtcNow;
}