using TaskManager.Domain.Tasks;
using TaskManager.Domain.Users;

namespace TaskManager.Domain.Lists;

/// <summary>
/// TaskList aggregate root — a named list of tasks shared among members.
/// The owner is automatically a member. All members have equal permissions.
/// </summary>
public sealed class TaskList
{
    private readonly HashSet<UserId> _members = [];
    private readonly HashSet<TaskId> _taskIds = [];

    private TaskList(TaskListId id, string name, UserId ownerId, DateTime createdAt)
    {
        Id = id;
        Name = name;
        OwnerId = ownerId;
        CreatedAt = createdAt;
        _members.Add(ownerId);
    }

    public TaskListId Id { get; }
    public string Name { get; private set; }
    public UserId OwnerId { get; }
    public DateTime CreatedAt { get; }

    public IReadOnlySet<UserId> Members => _members;
    public IReadOnlySet<TaskId> TaskIds => _taskIds;

    /// <summary>
    /// Creates a new task list. The owner is automatically added as the first member.
    /// </summary>
    public static TaskList Create(string name, UserId ownerId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentNullException.ThrowIfNull(ownerId);

        return new TaskList(TaskListId.New(), name, ownerId, DateTime.UtcNow);
    }

    /// <summary>
    /// Adds a member to the list. Idempotent — re-adding an existing member is a no-op.
    /// </summary>
    public void AddMember(UserId memberId)
    {
        ArgumentNullException.ThrowIfNull(memberId);
        _members.Add(memberId);
    }

    /// <summary>
    /// Removes a member from the list. The owner cannot be removed.
    /// </summary>
    public void RemoveMember(UserId memberId)
    {
        ArgumentNullException.ThrowIfNull(memberId);
        if (memberId == OwnerId)
            throw new InvalidOperationException("The owner cannot be removed from the list.");

        _members.Remove(memberId);
    }

    /// <summary>
    /// Adds a task to the list. The requester must be a member.
    /// </summary>
    public void AddTask(TaskId taskId, UserId requesterId)
    {
        ArgumentNullException.ThrowIfNull(taskId);
        EnsureMember(requesterId);
        _taskIds.Add(taskId);
    }

    /// <summary>
    /// Removes a task from the list. The requester must be a member.
    /// </summary>
    public void RemoveTask(TaskId taskId, UserId requesterId)
    {
        ArgumentNullException.ThrowIfNull(taskId);
        EnsureMember(requesterId);
        _taskIds.Remove(taskId);
    }

    private void EnsureMember(UserId userId)
    {
        ArgumentNullException.ThrowIfNull(userId);
        if (!_members.Contains(userId))
            throw new InvalidOperationException($"User '{userId.Value}' is not a member of this list.");
    }
}
