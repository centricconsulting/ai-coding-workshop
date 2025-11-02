using TaskManager.Domain.Tasks;

namespace TaskManager.Application.Commands;

/// <summary>
/// Command to update an existing task
/// </summary>
public sealed record UpdateTaskCommand(
    TaskId TaskId,
    string Title,
    string Description,
    TaskPriority Priority,
    DateTime? DueDate
);
