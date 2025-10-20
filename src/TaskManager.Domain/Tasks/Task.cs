namespace TaskManager.Domain.Tasks;

using TaskManager.Domain.ValueObjects;

/// <summary>
/// Task aggregate root representing a work item to be completed
/// </summary>
public sealed class Task
{
    private Task(
        TaskId id, 
        string title, 
        string description,
        Priority priority,
        DateTime? dueDate,
        TaskStatus status, 
        DateTime createdAt)
    {
        Id = id;
        Title = title;
        Description = description;
        Priority = priority;
        DueDate = dueDate;
        Status = status;
        CreatedAt = createdAt;
        UpdatedAt = createdAt;
    }

    public TaskId Id { get; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public Priority Priority { get; private set; }
    public DateTime? DueDate { get; private set; }
    public TaskStatus Status { get; private set; }
    public DateTime CreatedAt { get; }
    public DateTime UpdatedAt { get; private set; }

    /// <summary>
    /// Factory method to create a new task
    /// </summary>
    public static Task Create(string title, string description, Priority priority, DateTime? dueDate = null)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be null or empty", nameof(title));

        if (priority is null)
            throw new ArgumentNullException(nameof(priority));

        if (dueDate.HasValue && dueDate.Value <= DateTime.UtcNow)
            throw new ArgumentException("Due date must be in the future", nameof(dueDate));
        
        return new Task(
            TaskId.New(),
            title,
            description,
            priority,
            dueDate,
            TaskStatus.Todo,
            DateTime.UtcNow);
    }

    /// <summary>
    /// Business method to update task priority
    /// </summary>
    public void UpdatePriority(Priority priority)
    {
        if (priority is null)
            throw new ArgumentNullException(nameof(priority));

        Priority = priority;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Business method to update task due date
    /// </summary>
    public void UpdateDueDate(DateTime? dueDate)
    {
        if (dueDate.HasValue && dueDate.Value <= DateTime.UtcNow)
            throw new ArgumentException("Due date must be in the future", nameof(dueDate));

        DueDate = dueDate;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Business method to update task status
    /// </summary>
    public void UpdateStatus(TaskStatus newStatus)
    {
        Status = newStatus;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Business method to update task details
    /// </summary>
    public void UpdateDetails(string title, string description)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be null or empty", nameof(title));
        
        Title = title;
        Description = description;
        UpdatedAt = DateTime.UtcNow;
    }
}
