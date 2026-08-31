namespace TaskManager.Domain.Tasks;

/// <summary>
/// Strongly-typed identifier for Task entities
/// </summary>
public sealed record TaskId(Guid Value)
{
    public static TaskId New() => new(Guid.NewGuid());
    public static TaskId From(Guid value) => new(value);
    
    public override string ToString() => Value.ToString();
}
