namespace TaskManager.Api.Models;

/// <summary>
/// Response DTO for task data
/// Maps domain Task entity to HTTP response
/// </summary>
public sealed record TaskResponse
{
    /// <summary>
    /// Unique identifier for the task
    /// </summary>
    public required Guid Id { get; init; }

    /// <summary>
    /// The title of the task
    /// </summary>
    public required string Title { get; init; }

    /// <summary>
    /// The description of the task
    /// </summary>
    public required string Description { get; init; }

    /// <summary>
    /// Current status of the task (Todo, InProgress, Done, Cancelled)
    /// </summary>
    public required string Status { get; init; }

    /// <summary>
    /// Priority level (Low, Medium, High, Critical)
    /// </summary>
    public required string Priority { get; init; }

    /// <summary>
    /// Optional due date for the task
    /// </summary>
    public DateTime? DueDate { get; init; }

    /// <summary>
    /// Indicates if the task is completed
    /// </summary>
    public required bool IsCompleted { get; init; }

    /// <summary>
    /// Timestamp when the task was completed (if applicable)
    /// </summary>
    public DateTime? CompletedAt { get; init; }

    /// <summary>
    /// Timestamp when the task was created
    /// </summary>
    public required DateTime CreatedAt { get; init; }

    /// <summary>
    /// Timestamp when the task was last updated
    /// </summary>
    public required DateTime UpdatedAt { get; init; }

    /// <summary>
    /// Maps a domain Task entity to a TaskResponse DTO
    /// </summary>
    /// <param name="task">The domain task entity</param>
    /// <returns>A TaskResponse DTO</returns>
    public static TaskResponse FromDomain(Domain.Tasks.Task task)
    {
        if (task == null)
        {
            throw new ArgumentNullException(nameof(task));
        }

        return new TaskResponse
        {
            Id = task.Id.Value,
            Title = task.Title,
            Description = task.Description,
            Status = task.Status.ToString(),
            Priority = task.Priority.Name,
            DueDate = task.DueDate,
            IsCompleted = task.IsCompleted,
            CompletedAt = task.CompletedAt,
            CreatedAt = task.CreatedAt,
            UpdatedAt = task.UpdatedAt
        };
    }
}
