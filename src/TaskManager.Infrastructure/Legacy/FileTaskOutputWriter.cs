using Microsoft.Extensions.Logging;

namespace TaskManager.Infrastructure.Legacy;

/// <summary>
/// File-based implementation of ITaskOutputWriter
/// Writes task output to text files in the current directory
/// </summary>
public sealed class FileTaskOutputWriter : ITaskOutputWriter
{
    private readonly ILogger<FileTaskOutputWriter> _logger;

    public FileTaskOutputWriter(ILogger<FileTaskOutputWriter> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Writes task output to a file named "task_{taskId}.txt"
    /// </summary>
    public async Task WriteTaskOutputAsync(
        int taskId,
        string content,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(content);

        var fileName = $"task_{taskId}.txt";

        _logger.LogDebug(
            "Writing task output to file {FileName}, content length: {Length}",
            fileName,
            content.Length);

        try
        {
            await File.WriteAllTextAsync(fileName, content, cancellationToken);

            _logger.LogInformation(
                "Successfully wrote task {TaskId} output to file {FileName}",
                taskId,
                fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to write task {TaskId} output to file {FileName}",
                taskId,
                fileName);
            throw;
        }
    }
}
