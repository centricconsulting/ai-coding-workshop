using TaskManager.Domain.Tasks;
using DomainTask = TaskManager.Domain.Tasks.Task;

namespace TaskManager.Domain.Repositories;

/// <summary>
/// Repository interface for Task aggregate
/// Uses business-intent method names (not generic CRUD)
/// </summary>
public interface ITaskRepository
{
    // TODO: Participants will implement these methods during the workshop
    // Note: Using business-intent names, not generic CRUD operations
    
    System.Threading.Tasks.Task<DomainTask?> FindByIdAsync(TaskId taskId, CancellationToken cancellationToken = default);
    
    System.Threading.Tasks.Task<IEnumerable<DomainTask>> GetActiveTasksAsync(CancellationToken cancellationToken = default);
    
    System.Threading.Tasks.Task AddTaskAsync(DomainTask task, CancellationToken cancellationToken = default);
    
    System.Threading.Tasks.Task SaveChangesAsync(DomainTask task, CancellationToken cancellationToken = default);
    
    System.Threading.Tasks.Task<bool> RemoveTaskAsync(TaskId taskId, CancellationToken cancellationToken = default);
}
