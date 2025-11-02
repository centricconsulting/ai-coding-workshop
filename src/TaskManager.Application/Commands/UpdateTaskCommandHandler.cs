using Microsoft.Extensions.Logging;
using TaskManager.Domain.Repositories;
using TaskManager.Domain.Tasks;
using DomainTask = TaskManager.Domain.Tasks.Task;

namespace TaskManager.Application.Commands;

/// <summary>
/// Handler for updating an existing task
/// </summary>
public sealed class UpdateTaskCommandHandler
{
    private readonly ITaskRepository _repository;
    private readonly ILogger<UpdateTaskCommandHandler> _logger;

    public UpdateTaskCommandHandler(
        ITaskRepository repository,
        ILogger<UpdateTaskCommandHandler> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Handles the update task command
    /// </summary>
    /// <param name="command">The command containing task update details</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated task, or null if not found</returns>
    /// <exception cref="ArgumentNullException">Thrown when command is null</exception>
    /// <exception cref="ArgumentException">Thrown when validation fails (e.g., past due date)</exception>
    public async System.Threading.Tasks.Task<DomainTask?> HandleAsync(
        UpdateTaskCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command == null)
        {
            throw new ArgumentNullException(nameof(command));
        }

        _logger.LogInformation(
            "Updating task with ID {TaskId}, Title '{Title}', Priority {Priority}",
            command.TaskId,
            command.Title,
            command.Priority);

        // Find existing task
        var task = await _repository.FindByIdAsync(command.TaskId, cancellationToken);
        
        if (task == null)
        {
            _logger.LogWarning("Task with ID {TaskId} not found", command.TaskId);
            return null;
        }

        try
        {
            // Update task details
            task.UpdateDetails(command.Title, command.Description);
            task.UpdatePriority(command.Priority);
            task.UpdateDueDate(command.DueDate);

            // Persist changes
            await _repository.SaveChangesAsync(task, cancellationToken);

            _logger.LogInformation(
                "Task with ID {TaskId} updated successfully. Priority: {Priority}, DueDate: {DueDate}",
                command.TaskId,
                command.Priority,
                command.DueDate);

            return task;
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(
                ex,
                "Failed to update task with ID {TaskId} due to validation error",
                command.TaskId);
            throw;
        }
    }
}
