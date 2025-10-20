using Microsoft.Extensions.Logging;
using TaskManager.Domain.Repositories;
using TaskManager.Domain.Tasks;
using DomainTask = TaskManager.Domain.Tasks.Task;
using TaskStatus = TaskManager.Domain.Tasks.TaskStatus;
using System.Diagnostics;

namespace TaskManager.Application.Services;

/// <summary>
/// Application service for task management
/// This will be used in Lab 2: Requirements → Backlog → Code
/// 
/// Includes OpenTelemetry ActivitySource for observability demonstrations
/// </summary>
public sealed class TaskService
{
    private readonly ITaskRepository _taskRepository;
    private readonly ILogger<TaskService> _logger;
    private static readonly ActivitySource ActivitySource = new("TaskManager.Application");

    public TaskService(ITaskRepository taskRepository, ILogger<TaskService> logger)
    {
        _taskRepository = taskRepository;
        _logger = logger;
    }

    /// <summary>
    /// Add a new task to the system
    /// TODO: This method will be implemented during Lab 2 with Copilot assistance
    /// 
    /// Note: Includes OpenTelemetry activity for observability demonstration
    /// </summary>
    public System.Threading.Tasks.Task<TaskId> AddTaskAsync(string title, string description, CancellationToken cancellationToken = default)
    {
        using var activity = ActivitySource.StartActivity("TaskService.AddTask");
        activity?.SetTag("task.title", title);
        activity?.SetTag("operation", "create");
        
        // TODO: Participants will implement this during the workshop
        // Expected implementation:
        // 1. Log the operation
        // 2. Create task using domain factory
        // 3. Add to repository
        // 4. Return task ID
        // 5. Handle validation errors appropriately
        // 6. Add activity tags for observability
        
        _logger.LogInformation("AddTaskAsync called with title: {Title}", title);
        throw new NotImplementedException("This will be implemented during Lab 2");
    }

    /// <summary>
    /// Get a task by its ID
    /// TODO: This method will be implemented during the workshop
    /// </summary>
    public System.Threading.Tasks.Task<DomainTask?> GetTaskAsync(TaskId taskId, CancellationToken cancellationToken = default)
    {
        // TODO: Participants will implement this during the workshop
        throw new NotImplementedException("This will be implemented during the workshop");
    }

    /// <summary>
    /// Update task status
    /// TODO: This method will be implemented during the workshop
    /// </summary>
    public System.Threading.Tasks.Task UpdateTaskStatusAsync(TaskId taskId, TaskStatus newStatus, CancellationToken cancellationToken = default)
    {
        // TODO: Participants will implement this during the workshop
        throw new NotImplementedException("This will be implemented during the workshop");
    }

    /// <summary>
    /// Get all active tasks
    /// TODO: This method will be implemented during the workshop
    /// </summary>
    public System.Threading.Tasks.Task<IEnumerable<DomainTask>> GetActiveTasksAsync(CancellationToken cancellationToken = default)
    {
        // TODO: Participants will implement this during the workshop
        throw new NotImplementedException("This will be implemented during the workshop");
    }
}
