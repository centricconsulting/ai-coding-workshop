namespace TaskManager.Application.Commands;

/// <summary>
/// Command to create a new task with priority and optional due date.
/// </summary>
public sealed record CreateTaskCommand
{
    /// <summary>
    /// Gets the task title.
    /// </summary>
    public required string Title { get; init; }

    /// <summary>
    /// Gets the task description.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Gets the priority level as a string (Low, Medium, High, Critical).
    /// </summary>
    public required string Priority { get; init; }

    /// <summary>
    /// Gets the optional due date for the task.
    /// </summary>
    public DateTime? DueDate { get; init; }
}
