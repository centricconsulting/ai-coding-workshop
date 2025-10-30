using Microsoft.Extensions.Logging;
using TaskManager.Domain.Repositories;
using TaskManager.Domain.Tasks;
using DomainTask = TaskManager.Domain.Tasks.Task;

namespace TaskManager.Application.Commands;

/// <summary>
/// Handles the creation of new tasks following CQRS pattern
/// </summary>
public sealed class CreateTaskCommandHandler
{
    private readonly ITaskRepository _repository;
    private readonly ILogger<CreateTaskCommandHandler> _logger;

    public CreateTaskCommandHandler(
        ITaskRepository repository,
        ILogger<CreateTaskCommandHandler> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Handles the CreateTaskCommand and creates a new task
    /// </summary>
    /// <param name="command">The command containing task creation details</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created task</returns>
    /// <exception cref="ArgumentNullException">Thrown when command is null</exception>
    /// <exception cref="ArgumentException">Thrown when priority is invalid or due date is in the past</exception>
    public async Task<DomainTask> HandleAsync(
        CreateTaskCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command == null)
        {
            throw new ArgumentNullException(nameof(command), "Command cannot be null.");
        }

        _logger.LogInformation(
            "Creating task with title '{Title}' and priority '{Priority}'",
            command.Title,
            command.Priority);

        // Parse priority string to TaskPriority value object
        TaskPriority priority;
        try
        {
            priority = TaskPriority.FromName(command.Priority);
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(
                ex,
                "Invalid priority '{Priority}' provided for task creation",
                command.Priority);
            throw new ArgumentException(
                $"Invalid priority '{command.Priority}'. Valid values are: Low, Medium, High, Critical.",
                nameof(command.Priority),
                ex);
        }

        // Create task entity using factory method (validates due date)
        DomainTask task;
        try
        {
            task = DomainTask.Create(
                command.Title,
                command.Description ?? string.Empty,
                priority,
                command.DueDate);
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(
                ex,
                "Failed to create task with title '{Title}' due to validation error",
                command.Title);
            throw;
        }

        // Save via repository
        await _repository.AddTaskAsync(task, cancellationToken);

        _logger.LogInformation(
            "Task created successfully with ID {TaskId}, Priority {Priority}, DueDate {DueDate}",
            task.Id,
            task.Priority.Name,
            task.DueDate);

        return task;
    }
}
