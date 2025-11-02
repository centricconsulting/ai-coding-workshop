using FakeItEasy;
using Microsoft.Extensions.Logging;
using TaskManager.Application.Commands;
using TaskManager.Domain.Repositories;
using TaskManager.Domain.Tasks;
using Xunit;

namespace TaskManager.UnitTests.Commands.DeleteTaskCommandHandlerTests;

public sealed class HandleAsyncTests
{
    [Fact]
    public async System.Threading.Tasks.Task HandleAsync_WithExistingTask_DeletesTaskSuccessfully()
    {
        // Arrange
        var repository = A.Fake<ITaskRepository>();
        var logger = A.Fake<ILogger<DeleteTaskCommandHandler>>();
        var handler = new DeleteTaskCommandHandler(repository, logger);

        var taskId = TaskId.New();
        A.CallTo(() => repository.RemoveTaskAsync(taskId, A<CancellationToken>._))
            .Returns(true);

        var command = new DeleteTaskCommand(taskId);

        // Act
        await handler.HandleAsync(command, CancellationToken.None);

        // Assert
        A.CallTo(() => repository.RemoveTaskAsync(taskId, A<CancellationToken>._))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async System.Threading.Tasks.Task HandleAsync_WithNonExistentTask_ThrowsInvalidOperationException()
    {
        // Arrange
        var repository = A.Fake<ITaskRepository>();
        var logger = A.Fake<ILogger<DeleteTaskCommandHandler>>();
        var handler = new DeleteTaskCommandHandler(repository, logger);

        var taskId = TaskId.New();
        A.CallTo(() => repository.RemoveTaskAsync(taskId, A<CancellationToken>._))
            .Returns(false);

        var command = new DeleteTaskCommand(taskId);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            async () => await handler.HandleAsync(command, CancellationToken.None));

        Assert.Contains("not found", exception.Message);
    }

    [Fact]
    public async System.Threading.Tasks.Task HandleAsync_WithNullCommand_ThrowsArgumentNullException()
    {
        // Arrange
        var repository = A.Fake<ITaskRepository>();
        var logger = A.Fake<ILogger<DeleteTaskCommandHandler>>();
        var handler = new DeleteTaskCommandHandler(repository, logger);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(
            async () => await handler.HandleAsync(null!, CancellationToken.None));
    }

    [Fact]
    public async System.Threading.Tasks.Task HandleAsync_PassesCancellationToken_ToRepository()
    {
        // Arrange
        var repository = A.Fake<ITaskRepository>();
        var logger = A.Fake<ILogger<DeleteTaskCommandHandler>>();
        var handler = new DeleteTaskCommandHandler(repository, logger);

        var taskId = TaskId.New();
        A.CallTo(() => repository.RemoveTaskAsync(taskId, A<CancellationToken>._))
            .Returns(true);

        var command = new DeleteTaskCommand(taskId);
        var cts = new CancellationTokenSource();

        // Act
        await handler.HandleAsync(command, cts.Token);

        // Assert
        A.CallTo(() => repository.RemoveTaskAsync(taskId, cts.Token))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async System.Threading.Tasks.Task HandleAsync_LogsInformation_WhenTaskDeleted()
    {
        // Arrange
        var repository = A.Fake<ITaskRepository>();
        var logger = A.Fake<ILogger<DeleteTaskCommandHandler>>();
        var handler = new DeleteTaskCommandHandler(repository, logger);

        var taskId = TaskId.New();
        A.CallTo(() => repository.RemoveTaskAsync(taskId, A<CancellationToken>._))
            .Returns(true);

        var command = new DeleteTaskCommand(taskId);

        // Act
        await handler.HandleAsync(command, CancellationToken.None);

        // Assert
        A.CallTo(logger)
            .Where(call => call.Method.Name == "Log" && call.GetArgument<LogLevel>(0) == LogLevel.Information)
            .MustHaveHappened();
    }

    [Fact]
    public async System.Threading.Tasks.Task HandleAsync_LogsWarning_WhenTaskNotFound()
    {
        // Arrange
        var repository = A.Fake<ITaskRepository>();
        var logger = A.Fake<ILogger<DeleteTaskCommandHandler>>();
        var handler = new DeleteTaskCommandHandler(repository, logger);

        var taskId = TaskId.New();
        A.CallTo(() => repository.RemoveTaskAsync(taskId, A<CancellationToken>._))
            .Returns(false);

        var command = new DeleteTaskCommand(taskId);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(
            async () => await handler.HandleAsync(command, CancellationToken.None));

        A.CallTo(logger)
            .Where(call => call.Method.Name == "Log" && call.GetArgument<LogLevel>(0) == LogLevel.Warning)
            .MustHaveHappened();
    }
}
