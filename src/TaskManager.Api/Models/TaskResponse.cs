namespace TaskManager.Api.Models;

/// <summary>
/// Response model for task operations, mapping domain Task to API representation
/// </summary>
public sealed record TaskResponse
{
    /// <summary>
    /// Gets the unique task identifier.
    /// </summary>
    public required string Id { get; init; }

    /// <summary>
    /// Gets the task title.
    /// </summary>
    public required string Title { get; init; }

    /// <summary>
    /// Gets the task description.
    /// </summary>
    public required string Description { get; init; }

    /// <summary>
    /// Gets the priority level name (Low, Medium, High, Critical).
    /// </summary>
    public required string Priority { get; init; }

    /// <summary>
    /// Gets the task status (Todo, InProgress, Done).
    /// </summary>
    public required string Status { get; init; }

    /// <summary>
    /// Gets the optional due date.
    /// </summary>
    public DateTime? DueDate { get; init; }

    /// <summary>
    /// Gets the creation timestamp.
    /// </summary>
    public required DateTime CreatedAt { get; init; }

    /// <summary>
    /// Gets the last update timestamp.
    /// </summary>
    public required DateTime UpdatedAt { get; init; }
}
