namespace TaskManager.Domain.Tasks;

/// <summary>
/// Task aggregate root representing a work item to be completed
/// </summary>
public sealed class Task
{
    private Task(
        TaskId id, 
        string title, 
        string description, 
        TaskStatus status, 
        TaskPriority priority,
        DateTime? dueDate,
        DateTime createdAt)
    {
        Id = id;
        Title = title;
        Description = description;
        Status = status;
        Priority = priority;
        DueDate = dueDate;
        CreatedAt = createdAt;
        UpdatedAt = createdAt;
    }

    public TaskId Id { get; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public TaskStatus Status { get; private set; }
    public TaskPriority Priority { get; private set; }
    public DateTime? DueDate { get; private set; }
    public bool IsCompleted => Status == TaskStatus.Done;
    public DateTime? CompletedAt { get; private set; }
    public DateTime CreatedAt { get; }
    public DateTime UpdatedAt { get; private set; }

    /// <summary>
    /// Factory method to create a new task
    /// </summary>
    public static Task Create(string title, string description, TaskPriority? priority = null, DateTime? dueDate = null)
    {
        // Validate title
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Title cannot be null or empty.", nameof(title));
        }

        // Validate due date must be in future if provided
        if (dueDate.HasValue && dueDate.Value < DateTime.UtcNow)
        {
            throw new ArgumentException("Due date cannot be in the past.", nameof(dueDate));
        }

        // Use default priority if not provided
        var taskPriority = priority ?? TaskPriority.Medium;
        
        return new Task(
            TaskId.New(),
            title,
            description ?? string.Empty,
            TaskStatus.Todo,
            taskPriority,
            dueDate,
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

    /// <summary>
    /// Business method to update task priority
    /// </summary>
    public void UpdatePriority(TaskPriority priority)
    {
        if (priority == null)
        {
            throw new ArgumentNullException(nameof(priority), "Priority cannot be null.");
        }

        Priority = priority;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Business method to update task due date
    /// </summary>
    public void UpdateDueDate(DateTime? dueDate)
    {
        // Validate due date must be in future if provided
        if (dueDate.HasValue && dueDate.Value < DateTime.UtcNow)
        {
            throw new ArgumentException("Due date cannot be in the past.", nameof(dueDate));
        }

        DueDate = dueDate;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Business method to mark task as completed
    /// </summary>
    public void MarkAsCompleted()
    {
        Status = TaskStatus.Done;
        CompletedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
}
