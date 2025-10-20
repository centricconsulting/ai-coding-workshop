namespace TaskManager.Domain.ValueObjects;

/// <summary>
/// Represents task priority levels as a value object.
/// Implements DDD value object pattern with immutability and factory methods.
/// </summary>
public sealed record Priority
{
    public static readonly Priority Low = new(0, nameof(Low));
    public static readonly Priority Medium = new(1, nameof(Medium));
    public static readonly Priority High = new(2, nameof(High));
    public static readonly Priority Critical = new(3, nameof(Critical));

    /// <summary>
    /// Gets the numeric value of the priority.
    /// </summary>
    public int Value { get; }

    /// <summary>
    /// Gets the name of the priority.
    /// </summary>
    public string Name { get; }

    private Priority(int value, string name)
    {
        Value = value;
        Name = name;
    }

    /// <summary>
    /// Creates a Priority from a numeric value.
    /// </summary>
    /// <param name="value">The numeric priority value (0-3).</param>
    /// <returns>The corresponding Priority instance.</returns>
    /// <exception cref="ArgumentException">Thrown when value is not valid.</exception>
    public static Priority FromValue(int value) => value switch
    {
        0 => Low,
        1 => Medium,
        2 => High,
        3 => Critical,
        _ => throw new ArgumentException($"Invalid priority value: {value}", nameof(value))
    };

    /// <summary>
    /// Creates a Priority from a name string.
    /// </summary>
    /// <param name="name">The priority name (case-insensitive).</param>
    /// <returns>The corresponding Priority instance.</returns>
    /// <exception cref="ArgumentException">Thrown when name is not valid.</exception>
    public static Priority FromName(string name) => name?.ToLowerInvariant() switch
    {
        "low" => Low,
        "medium" => Medium,
        "high" => High,
        "critical" => Critical,
        _ => throw new ArgumentException($"Invalid priority name: {name}", nameof(name))
    };
}
