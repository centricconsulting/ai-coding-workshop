using Microsoft.Extensions.Logging;
using TaskManager.Application.Ports;

namespace TaskManager.Infrastructure.Legacy;

public sealed class FileSystemTaskFileWriter(ILogger<FileSystemTaskFileWriter> logger) : ITaskFileWriter
{
    public async Task WriteAsync(int taskId, string content, CancellationToken cancellationToken = default)
    {
        var path = $"task_{taskId}.txt";
        await File.WriteAllTextAsync(path, content, cancellationToken);
        logger.LogInformation("Task {TaskId} written to {Path}", taskId, path);
    }
}
