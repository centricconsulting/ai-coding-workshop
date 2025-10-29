namespace TaskManager.Domain.Tasks;

/// <summary>
/// Value object representing task priority levels
/// </summary>
public sealed record TaskPriority : IComparable<TaskPriority>
{
    private TaskPriority(string name, int value)
    {
        Name = name;
        Value = value;
    }

    /// <summary>
    /// The display name of the priority
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// The numeric value of the priority (higher = more urgent)
    /// </summary>
    public int Value { get; }

    /// <summary>
    /// Low priority (1)
    /// </summary>
    public static TaskPriority Low { get; } = new("Low", 1);

    /// <summary>
    /// Medium priority (2)
    /// </summary>
    public static TaskPriority Medium { get; } = new("Medium", 2);

    /// <summary>
    /// High priority (3)
    /// </summary>
    public static TaskPriority High { get; } = new("High", 3);

    /// <summary>
    /// Critical priority (4)
    /// </summary>
    public static TaskPriority Critical { get; } = new("Critical", 4);

    /// <summary>
    /// Creates a TaskPriority from a numeric value
    /// </summary>
    /// <param name="value">The numeric priority value (1-4)</param>
    /// <returns>The corresponding TaskPriority</returns>
    /// <exception cref="ArgumentException">Thrown when value is not between 1 and 4</exception>
    public static TaskPriority FromValue(int value)
    {
        return value switch
        {
            1 => Low,
            2 => Medium,
            3 => High,
            4 => Critical,
            _ => throw new ArgumentException($"Invalid priority value: {value}. Must be between 1 and 4.", nameof(value))
        };
    }

    /// <summary>
    /// Creates a TaskPriority from a string name (case-insensitive)
    /// </summary>
    /// <param name="name">The priority name (Low, Medium, High, Critical)</param>
    /// <returns>The corresponding TaskPriority</returns>
    /// <exception cref="ArgumentException">Thrown when name is null, empty, or invalid</exception>
    public static TaskPriority FromName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Priority name cannot be null or empty.", nameof(name));

        return name.Trim().ToLowerInvariant() switch
        {
            "low" => Low,
            "medium" => Medium,
            "high" => High,
            "critical" => Critical,
            _ => throw new ArgumentException($"Invalid priority name: '{name}'. Valid values are: Low, Medium, High, Critical.", nameof(name))
        };
    }

    /// <summary>
    /// Compares this priority with another for sorting
    /// </summary>
    public int CompareTo(TaskPriority? other)
    {
        if (other is null)
            return 1;

        return Value.CompareTo(other.Value);
    }

    /// <summary>
    /// Implicit conversion to int for convenience
    /// </summary>
    public static implicit operator int(TaskPriority priority) => priority.Value;

    public static bool operator >(TaskPriority left, TaskPriority right) => left.Value > right.Value;
    public static bool operator <(TaskPriority left, TaskPriority right) => left.Value < right.Value;
    public static bool operator >=(TaskPriority left, TaskPriority right) => left.Value >= right.Value;
    public static bool operator <=(TaskPriority left, TaskPriority right) => left.Value <= right.Value;

    public override string ToString() => Name;
}
