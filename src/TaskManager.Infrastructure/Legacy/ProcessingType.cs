namespace TaskManager.Infrastructure.Legacy;

/// <summary>
/// Defines the transformation strategy applied by <see cref="TaskProcessor"/>.
/// </summary>
public enum ProcessingType
{
    /// <summary>Inverts the case of each letter and replaces spaces with underscores. Result is truncated to 50 characters.</summary>
    NormalizeAndInvertCase,

    /// <summary>Converts all characters to uppercase.</summary>
    Uppercase,

    /// <summary>Preserves the first word and lowercases all subsequent words.</summary>
    SentenceCase,

    /// <summary>Returns the input data unchanged.</summary>
    Passthrough
}
