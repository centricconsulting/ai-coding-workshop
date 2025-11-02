using TaskManager.Domain.Repositories;
using TaskManager.Domain.Tasks;
using DomainTask = TaskManager.Domain.Tasks.Task;
using TaskStatus = TaskManager.Domain.Tasks.TaskStatus;

namespace TaskManager.Infrastructure.Repositories;

/// <summary>
/// In-memory implementation of ITaskRepository for workshop purposes
/// In a real application, this would be an Entity Framework or other data access implementation
/// </summary>
public sealed class InMemoryTaskRepository : ITaskRepository
{
    private readonly Dictionary<TaskId, DomainTask> _tasks = new();

    public System.Threading.Tasks.Task<DomainTask?> FindByIdAsync(TaskId taskId, CancellationToken cancellationToken = default)
    {
        _tasks.TryGetValue(taskId, out var task);
        return System.Threading.Tasks.Task.FromResult(task);
    }

    public System.Threading.Tasks.Task<IEnumerable<DomainTask>> GetActiveTasksAsync(CancellationToken cancellationToken = default)
    {
        var activeTasks = _tasks.Values.Where(t => t.Status != TaskStatus.Done && t.Status != TaskStatus.Cancelled);
        return System.Threading.Tasks.Task.FromResult(activeTasks);
    }

    public System.Threading.Tasks.Task AddTaskAsync(DomainTask task, CancellationToken cancellationToken = default)
    {
        _tasks[task.Id] = task;
        return System.Threading.Tasks.Task.CompletedTask;
    }

    public System.Threading.Tasks.Task SaveChangesAsync(DomainTask task, CancellationToken cancellationToken = default)
    {
        _tasks[task.Id] = task;
        return System.Threading.Tasks.Task.CompletedTask;
    }

    public System.Threading.Tasks.Task<bool> RemoveTaskAsync(TaskId taskId, CancellationToken cancellationToken = default)
    {
        var removed = _tasks.Remove(taskId);
        return System.Threading.Tasks.Task.FromResult(removed);
    }
}
