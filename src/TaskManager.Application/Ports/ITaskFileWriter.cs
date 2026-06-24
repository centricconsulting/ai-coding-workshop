namespace TaskManager.Application.Ports;

/// <summary>
/// Port for persisting processed task output.
/// </summary>
public interface ITaskFileWriter
{
    /// <summary>
    /// Writes the processed <paramref name="content"/> for the specified task to a persistent store.
    /// </summary>
    /// <param name="taskId">The unique identifier of the task.</param>
    /// <param name="content">The processed content to persist.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task WriteAsync(int taskId, string content, CancellationToken cancellationToken = default);
}
