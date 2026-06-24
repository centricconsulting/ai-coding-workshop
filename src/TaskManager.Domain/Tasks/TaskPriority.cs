namespace TaskManager.Domain.Tasks;

/// <summary>
/// Value object representing task priority on a scale of 1 (Low) to 5 (Critical)
/// </summary>
public sealed record TaskPriority
{
    public static readonly TaskPriority Low = new(1);
    public static readonly TaskPriority Normal = new(2);
    public static readonly TaskPriority High = new(3);
    public static readonly TaskPriority Urgent = new(4);
    public static readonly TaskPriority Critical = new(5);

    public int Value { get; }

    public TaskPriority(int value)
    {
        if (value is < 1 or > 5)
            throw new ArgumentOutOfRangeException(nameof(value), value, "Priority must be between 1 (Low) and 5 (Critical).");

        Value = value;
    }

    public override string ToString() => Value switch
    {
        1 => "Low",
        2 => "Normal",
        3 => "High",
        4 => "Urgent",
        5 => "Critical",
        _ => Value.ToString()
    };
}
