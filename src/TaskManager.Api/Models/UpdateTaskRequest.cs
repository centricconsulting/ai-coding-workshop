using System.ComponentModel.DataAnnotations;

namespace TaskManager.Api.Models;

/// <summary>
/// Request model for updating an existing task.
/// </summary>
public sealed record UpdateTaskRequest
{
    /// <summary>
    /// Gets or sets the title of the task.
    /// </summary>
    [Required(ErrorMessage = "Title is required")]
    [StringLength(200, MinimumLength = 1, ErrorMessage = "Title must be between 1 and 200 characters")]
    public string Title { get; init; } = string.Empty;

    /// <summary>
    /// Gets or sets the optional description of the task.
    /// </summary>
    [StringLength(2000, ErrorMessage = "Description cannot exceed 2000 characters")]
    public string? Description { get; init; }

    /// <summary>
    /// Gets or sets the priority of the task.
    /// </summary>
    [Required(ErrorMessage = "Priority is required")]
    [RegularExpression("^(Low|Medium|High|Critical)$", ErrorMessage = "Priority must be one of: Low, Medium, High, Critical")]
    public string Priority { get; init; } = string.Empty;

    /// <summary>
    /// Gets or sets the optional due date of the task.
    /// </summary>
    public DateTime? DueDate { get; init; }
}
