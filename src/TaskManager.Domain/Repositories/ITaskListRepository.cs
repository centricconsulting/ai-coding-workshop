using TaskManager.Domain.Lists;
using TaskManager.Domain.Users;
using DomainTaskList = TaskManager.Domain.Lists.TaskList;

namespace TaskManager.Domain.Repositories;

/// <summary>
/// Repository interface for the TaskList aggregate.
/// Uses business-intent method names (not generic CRUD).
/// </summary>
public interface ITaskListRepository
{
    System.Threading.Tasks.Task<DomainTaskList?> FindByIdAsync(TaskListId listId, CancellationToken cancellationToken = default);

    System.Threading.Tasks.Task<IEnumerable<DomainTaskList>> GetListsByMemberAsync(UserId memberId, CancellationToken cancellationToken = default);

    System.Threading.Tasks.Task AddTaskListAsync(DomainTaskList taskList, CancellationToken cancellationToken = default);

    System.Threading.Tasks.Task SaveChangesAsync(DomainTaskList taskList, CancellationToken cancellationToken = default);
}
