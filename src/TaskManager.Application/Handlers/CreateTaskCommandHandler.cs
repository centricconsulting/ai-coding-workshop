namespace TaskManager.Application.Handlers;

using Microsoft.Extensions.Logging;
using TaskManager.Application.Commands;
using TaskManager.Domain.Repositories;
using TaskManager.Domain.ValueObjects;
using DomainTask = TaskManager.Domain.Tasks.Task;
using DomainPriority = TaskManager.Domain.ValueObjects.Priority;

/// <summary>
/// Handler for CreateTaskCommand following CQRS pattern
/// Validates command, creates domain Task, persists via repository
/// </summary>
public sealed class CreateTaskCommandHandler
{
    private readonly ITaskRepository _taskRepository;
    private readonly ILogger<CreateTaskCommandHandler> _logger;

    public CreateTaskCommandHandler(ITaskRepository taskRepository, ILogger<CreateTaskCommandHandler> logger)
    {
        _taskRepository = taskRepository ?? throw new ArgumentNullException(nameof(taskRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Handles task creation command
    /// </summary>
    /// <param name="command">The command containing task details</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created task</returns>
    /// <exception cref="ArgumentNullException">When command is null</exception>
    /// <exception cref="ArgumentException">When priority is invalid or due date is in the past</exception>
    public async Task<DomainTask> HandleAsync(
        CreateTaskCommand command, 
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        cancellationToken.ThrowIfCancellationRequested();

        _logger.LogInformation(
            "Creating task with title '{Title}' and priority '{Priority}'",
            command.Title,
            command.Priority);

        // Parse priority from string using factory method
        var priority = DomainPriority.FromName(command.Priority);

        // Create task using domain factory method (validates title, priority, due date)
        var task = DomainTask.Create(
            command.Title,
            command.Description ?? string.Empty,
            priority,
            command.DueDate);

        // Persist via repository
        await _taskRepository.AddTaskAsync(task, cancellationToken);

        _logger.LogInformation(
            "Successfully created task {TaskId} with title '{Title}'",
            task.Id,
            task.Title);

        return task;
    }
}
