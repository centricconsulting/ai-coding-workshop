namespace TaskManager.Domain.Lists;

/// <summary>
/// Strongly-typed identifier for the TaskList aggregate
/// </summary>
public sealed record TaskListId(Guid Value)
{
    public static TaskListId New() => new(Guid.NewGuid());
    public static TaskListId From(Guid value) => new(value);

    public override string ToString() => Value.ToString();
}
