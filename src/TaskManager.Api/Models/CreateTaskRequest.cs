using System.ComponentModel.DataAnnotations;

namespace TaskManager.Api.Models;

/// <summary>
/// Request DTO for creating a new task
/// Maps HTTP request to application command
/// </summary>
public sealed record CreateTaskRequest
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
    /// Priority level: Low, Medium, High, or Critical (required)
    /// </summary>
    [Required(ErrorMessage = "Priority is required")]
    [RegularExpression("^(Low|Medium|High|Critical)$", 
        ErrorMessage = "Priority must be one of: Low, Medium, High, Critical")]
    public required string Priority { get; init; }

    /// <summary>
    /// Optional due date for the task (must be in future if provided)
    /// </summary>
    public DateTime? DueDate { get; init; }
}
