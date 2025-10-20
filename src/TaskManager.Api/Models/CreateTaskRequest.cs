namespace TaskManager.Api.Models;

/// <summary>
/// Request model for creating a new task via POST /tasks
/// </summary>
public sealed record CreateTaskRequest
{
    /// <summary>
    /// Gets the task title.
    /// </summary>
    public required string Title { get; init; }

    /// <summary>
    /// Gets the optional task description.
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
