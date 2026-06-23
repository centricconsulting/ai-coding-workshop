using System.Text;
using Microsoft.Extensions.Logging;
using TaskManager.Application.Ports;

namespace TaskManager.Infrastructure.Legacy;

/// <summary>
/// Processes task data by applying a named transformation strategy.
/// </summary>
public sealed class TaskProcessor(ILogger<TaskProcessor> logger, ITaskFileWriter fileWriter)
{
    private const int MAX_RESULT_LENGTH = 50;

    /// <summary>
    /// Applies the specified <paramref name="type"/> transformation to <paramref name="data"/> and returns the result.
    /// </summary>
    /// <param name="taskId">The unique identifier of the task being processed.</param>
    /// <param name="data">The input data to transform. Must not be null or empty.</param>
    /// <param name="type">The transformation to apply.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>The transformed string.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="data"/> is null or empty.</exception>
    public async Task<string> ProcessTaskAsync(
        int taskId,
        string data,
        ProcessingType type,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(data, nameof(data));

        logger.LogInformation("Processing task {TaskId} with type {ProcessingType}", taskId, type);

        var result = type switch
        {
            ProcessingType.NormalizeAndInvertCase => await NormalizeAndInvertCaseAsync(taskId, data, cancellationToken),
            ProcessingType.Uppercase              => data.ToUpper(),
            ProcessingType.SentenceCase           => ToSentenceCase(data),
            ProcessingType.Passthrough or _       => data
        };

        logger.LogInformation("Task {TaskId} processed successfully", taskId);
        return result;
    }

    private async Task<string> NormalizeAndInvertCaseAsync(
        int taskId,
        string data,
        CancellationToken cancellationToken)
    {
        var builder = new StringBuilder(data.Length);

        foreach (var ch in data)
            builder.Append(NormalizeChar(ch));

        if (builder.Length > MAX_RESULT_LENGTH)
            builder.Remove(MAX_RESULT_LENGTH, builder.Length - MAX_RESULT_LENGTH);

        var result = builder.ToString();

        await Task.Delay(100, cancellationToken);

        try
        {
            await fileWriter.WriteAsync(taskId, result, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to write task {TaskId} to file", taskId);
            throw;
        }

        return result;
    }

    private static char NormalizeChar(char ch) => ch switch
    {
        ' '                     => '_',
        _ when char.IsUpper(ch) => char.ToLower(ch),
        _                       => char.ToUpper(ch)
    };

    private static string ToSentenceCase(string data)
    {
        var words = data.Split(' ');
        var builder = new StringBuilder(words[0]);

        for (var i = 1; i < words.Length; i++)
            builder.Append(' ').Append(words[i].ToLower());

        return builder.ToString();
    }
}
