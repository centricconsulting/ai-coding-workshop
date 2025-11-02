using Microsoft.Extensions.Logging;
using TaskManager.Domain.Repositories;
using TaskManager.Domain.Tasks;
using DomainTask = TaskManager.Domain.Tasks.Task;

namespace TaskManager.Application.Queries;

/// <summary>
/// Handler for GetTasksQuery that retrieves tasks with optional filtering
/// </summary>
public sealed class GetTasksQueryHandler
{
    private readonly ITaskRepository _repository;
    private readonly ILogger<GetTasksQueryHandler> _logger;

    public GetTasksQueryHandler(
        ITaskRepository repository,
        ILogger<GetTasksQueryHandler> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Handles the GetTasksQuery and returns filtered, ordered tasks
    /// </summary>
    public async Task<IEnumerable<DomainTask>> HandleAsync(
        GetTasksQuery query,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(query);

        _logger.LogInformation(
            "Retrieving tasks with status filter: {Status}",
            query.Status?.ToString() ?? "None");

        // Get all active tasks from repository
        var tasks = await _repository.GetActiveTasksAsync(cancellationToken);

        // Apply status filter if provided
        if (query.Status.HasValue)
        {
            tasks = tasks.Where(t => t.Status == query.Status.Value);
            
            _logger.LogInformation(
                "Filtered tasks by status {Status}, found {Count} tasks",
                query.Status.Value,
                tasks.Count());
        }
        else
        {
            _logger.LogInformation(
                "No status filter applied, returning {Count} tasks",
                tasks.Count());
        }

        // Order by CreatedAt descending (most recent first)
        var orderedTasks = tasks.OrderByDescending(t => t.CreatedAt).ToList();

        return orderedTasks;
    }
}
