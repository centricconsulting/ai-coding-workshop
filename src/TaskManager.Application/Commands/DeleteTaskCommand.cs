using TaskManager.Domain.Tasks;

namespace TaskManager.Application.Commands;

/// <summary>
/// Command to delete an existing task
/// </summary>
public sealed record DeleteTaskCommand(TaskId TaskId);
