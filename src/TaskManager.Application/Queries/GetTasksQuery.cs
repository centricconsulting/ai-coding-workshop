using DomainTaskStatus = TaskManager.Domain.Tasks.TaskStatus;

namespace TaskManager.Application.Queries;

/// <summary>
/// Query to retrieve tasks with optional status filtering
/// </summary>
public sealed record GetTasksQuery
{
    /// <summary>
    /// Optional status filter. If null, returns all tasks regardless of status.
    /// </summary>
    public DomainTaskStatus? Status { get; init; }
}
