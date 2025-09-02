namespace TaskManager.Domain.Tasks;

/// <summary>
/// Task aggregate root representing a work item to be completed
/// </summary>
public sealed class Task
{
    private Task(TaskId id, string title, string description, TaskStatus status, DateTime createdAt)
    {
        Id = id;
        Title = title;
        Description = description;
        Status = status;
        CreatedAt = createdAt;
        UpdatedAt = createdAt;
    }

    public TaskId Id { get; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public TaskStatus Status { get; private set; }
    public DateTime CreatedAt { get; }
    public DateTime UpdatedAt { get; private set; }

    /// <summary>
    /// Factory method to create a new task
    /// </summary>
    public static Task Create(string title, string description)
    {
        // TODO: Add validation (title not null/empty, description not null)
        // This is where Copilot will help participants implement validation
        
        return new Task(
            TaskId.New(),
            title,
            description,
            TaskStatus.Todo,
            DateTime.UtcNow);
    }

    /// <summary>
    /// Business method to update task status
    /// </summary>
    public void UpdateStatus(TaskStatus newStatus)
    {
        // TODO: Add business rules (e.g., can't move from Done to Todo directly)
        // This will be implemented during the workshop
        
        Status = newStatus;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Business method to update task details
    /// </summary>
    public void UpdateDetails(string title, string description)
    {
        // TODO: Add validation
        // This will be implemented during the workshop
        
        Title = title;
        Description = description;
        UpdatedAt = DateTime.UtcNow;
    }
}
