namespace TaskManager.Domain.Users;

/// <summary>
/// Strongly-typed identifier for a User
/// </summary>
public sealed record UserId(Guid Value)
{
    public static UserId New() => new(Guid.NewGuid());
    public static UserId From(Guid value) => new(value);

    public override string ToString() => Value.ToString();
}
