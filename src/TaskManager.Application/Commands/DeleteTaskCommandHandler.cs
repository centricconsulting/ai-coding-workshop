using Microsoft.Extensions.Logging;
using TaskManager.Domain.Repositories;

namespace TaskManager.Application.Commands;

/// <summary>
/// Handler for deleting an existing task
/// </summary>
public sealed class DeleteTaskCommandHandler
{
    private readonly ITaskRepository _repository;
    private readonly ILogger<DeleteTaskCommandHandler> _logger;

    public DeleteTaskCommandHandler(
        ITaskRepository repository,
        ILogger<DeleteTaskCommandHandler> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Handles the delete task command
    /// </summary>
    /// <param name="command">The command containing task ID to delete</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <exception cref="ArgumentNullException">Thrown when command is null</exception>
    /// <exception cref="InvalidOperationException">Thrown when task is not found</exception>
    public async System.Threading.Tasks.Task HandleAsync(
        DeleteTaskCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command == null)
        {
            throw new ArgumentNullException(nameof(command));
        }

        _logger.LogInformation("Deleting task with ID {TaskId}", command.TaskId);

        var removed = await _repository.RemoveTaskAsync(command.TaskId, cancellationToken);

        if (!removed)
        {
            _logger.LogWarning("Task with ID {TaskId} not found for deletion", command.TaskId);
            throw new InvalidOperationException($"Task with ID {command.TaskId} not found");
        }

        _logger.LogInformation("Task with ID {TaskId} deleted successfully", command.TaskId);
    }
}
