using System.ComponentModel.DataAnnotations;

namespace TaskManager.Application.Commands;

/// <summary>
/// Command to create a new task with priority and optional due date
/// </summary>
public sealed record CreateTaskCommand
{
    /// <summary>
    /// The title of the task (required)
    /// </summary>
    [Required(ErrorMessage = "Title is required")]
    [StringLength(200, MinimumLength = 1, ErrorMessage = "Title must be between 1 and 200 characters")]
    public required string Title { get; init; }

    /// <summary>
    /// Optional description of the task
    /// </summary>
    [StringLength(2000, ErrorMessage = "Description cannot exceed 2000 characters")]
    public string? Description { get; init; }

    /// <summary>
    /// Priority level as string (Low, Medium, High, Critical)
    /// </summary>
    [Required(ErrorMessage = "Priority is required")]
    public required string Priority { get; init; }

    /// <summary>
    /// Optional due date for the task (must be in future if provided)
    /// </summary>
    public DateTime? DueDate { get; init; }
}
