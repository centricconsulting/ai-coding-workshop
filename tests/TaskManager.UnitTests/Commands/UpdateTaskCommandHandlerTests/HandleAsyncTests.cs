using FakeItEasy;
using Microsoft.Extensions.Logging;
using TaskManager.Application.Commands;
using TaskManager.Domain.Repositories;
using TaskManager.Domain.Tasks;
using Xunit;
using DomainTask = TaskManager.Domain.Tasks.Task;

namespace TaskManager.UnitTests.Commands.UpdateTaskCommandHandlerTests;

public sealed class HandleAsyncTests
{
    [Fact]
    public async System.Threading.Tasks.Task HandleAsync_WithValidCommand_UpdatesTaskSuccessfully()
    {
        // Arrange
        var repository = A.Fake<ITaskRepository>();
        var logger = A.Fake<ILogger<UpdateTaskCommandHandler>>();
        var handler = new UpdateTaskCommandHandler(repository, logger);

        var taskId = TaskId.New();
        var existingTask = DomainTask.Create("Original Title", "Original Description", TaskPriority.Low, null);
        A.CallTo(() => repository.FindByIdAsync(taskId, A<CancellationToken>._))
            .Returns(existingTask);

        var command = new UpdateTaskCommand(
            taskId,
            "Updated Title",
            "Updated Description",
            TaskPriority.High,
            DateTime.UtcNow.AddDays(7));

        // Act
        var result = await handler.HandleAsync(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Updated Title", result.Title);
        Assert.Equal("Updated Description", result.Description);
        Assert.Equal(TaskPriority.High, result.Priority);
        Assert.NotNull(result.DueDate);
        A.CallTo(() => repository.SaveChangesAsync(existingTask, A<CancellationToken>._))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async System.Threading.Tasks.Task HandleAsync_WithNonExistentTask_ReturnsNull()
    {
        // Arrange
        var repository = A.Fake<ITaskRepository>();
        var logger = A.Fake<ILogger<UpdateTaskCommandHandler>>();
        var handler = new UpdateTaskCommandHandler(repository, logger);

        var taskId = TaskId.New();
        A.CallTo(() => repository.FindByIdAsync(taskId, A<CancellationToken>._))
            .Returns((DomainTask?)null);

        var command = new UpdateTaskCommand(
            taskId,
            "Updated Title",
            "Updated Description",
            TaskPriority.High,
            null);

        // Act
        var result = await handler.HandleAsync(command, CancellationToken.None);

        // Assert
        Assert.Null(result);
        A.CallTo(() => repository.SaveChangesAsync(A<DomainTask>._, A<CancellationToken>._))
            .MustNotHaveHappened();
    }

    [Fact]
    public async System.Threading.Tasks.Task HandleAsync_WithPastDueDate_ThrowsArgumentException()
    {
        // Arrange
        var repository = A.Fake<ITaskRepository>();
        var logger = A.Fake<ILogger<UpdateTaskCommandHandler>>();
        var handler = new UpdateTaskCommandHandler(repository, logger);

        var taskId = TaskId.New();
        var existingTask = DomainTask.Create("Original Title", "Original Description", TaskPriority.Low, null);
        A.CallTo(() => repository.FindByIdAsync(taskId, A<CancellationToken>._))
            .Returns(existingTask);

        var command = new UpdateTaskCommand(
            taskId,
            "Updated Title",
            "Updated Description",
            TaskPriority.High,
            DateTime.UtcNow.AddDays(-1)); // Past date

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(
            async () => await handler.HandleAsync(command, CancellationToken.None));

        A.CallTo(() => repository.SaveChangesAsync(A<DomainTask>._, A<CancellationToken>._))
            .MustNotHaveHappened();
    }

    [Fact]
    public async System.Threading.Tasks.Task HandleAsync_WithNullCommand_ThrowsArgumentNullException()
    {
        // Arrange
        var repository = A.Fake<ITaskRepository>();
        var logger = A.Fake<ILogger<UpdateTaskCommandHandler>>();
        var handler = new UpdateTaskCommandHandler(repository, logger);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(
            async () => await handler.HandleAsync(null!, CancellationToken.None));
    }

    [Fact]
    public async System.Threading.Tasks.Task HandleAsync_PassesCancellationToken_ToRepository()
    {
        // Arrange
        var repository = A.Fake<ITaskRepository>();
        var logger = A.Fake<ILogger<UpdateTaskCommandHandler>>();
        var handler = new UpdateTaskCommandHandler(repository, logger);

        var taskId = TaskId.New();
        var existingTask = DomainTask.Create("Original Title", "Original Description", TaskPriority.Low, null);
        A.CallTo(() => repository.FindByIdAsync(taskId, A<CancellationToken>._))
            .Returns(existingTask);

        var command = new UpdateTaskCommand(
            taskId,
            "Updated Title",
            "Updated Description",
            TaskPriority.High,
            null);

        var cts = new CancellationTokenSource();

        // Act
        await handler.HandleAsync(command, cts.Token);

        // Assert
        A.CallTo(() => repository.FindByIdAsync(taskId, cts.Token))
            .MustHaveHappenedOnceExactly();
        A.CallTo(() => repository.SaveChangesAsync(existingTask, cts.Token))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async System.Threading.Tasks.Task HandleAsync_LogsInformation_WhenTaskUpdated()
    {
        // Arrange
        var repository = A.Fake<ITaskRepository>();
        var logger = A.Fake<ILogger<UpdateTaskCommandHandler>>();
        var handler = new UpdateTaskCommandHandler(repository, logger);

        var taskId = TaskId.New();
        var existingTask = DomainTask.Create("Original Title", "Original Description", TaskPriority.Low, null);
        A.CallTo(() => repository.FindByIdAsync(taskId, A<CancellationToken>._))
            .Returns(existingTask);

        var command = new UpdateTaskCommand(
            taskId,
            "Updated Title",
            "Updated Description",
            TaskPriority.High,
            null);

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
        var logger = A.Fake<ILogger<UpdateTaskCommandHandler>>();
        var handler = new UpdateTaskCommandHandler(repository, logger);

        var taskId = TaskId.New();
        A.CallTo(() => repository.FindByIdAsync(taskId, A<CancellationToken>._))
            .Returns((DomainTask?)null);

        var command = new UpdateTaskCommand(
            taskId,
            "Updated Title",
            "Updated Description",
            TaskPriority.High,
            null);

        // Act
        await handler.HandleAsync(command, CancellationToken.None);

        // Assert
        A.CallTo(logger)
            .Where(call => call.Method.Name == "Log" && call.GetArgument<LogLevel>(0) == LogLevel.Warning)
            .MustHaveHappened();
    }

    [Fact]
    public async System.Threading.Tasks.Task HandleAsync_WithNullDueDate_ClearsExistingDueDate()
    {
        // Arrange
        var repository = A.Fake<ITaskRepository>();
        var logger = A.Fake<ILogger<UpdateTaskCommandHandler>>();
        var handler = new UpdateTaskCommandHandler(repository, logger);

        var taskId = TaskId.New();
        var existingTask = DomainTask.Create("Original Title", "Original Description", TaskPriority.Low, DateTime.UtcNow.AddDays(5));
        A.CallTo(() => repository.FindByIdAsync(taskId, A<CancellationToken>._))
            .Returns(existingTask);

        var command = new UpdateTaskCommand(
            taskId,
            "Updated Title",
            "Updated Description",
            TaskPriority.High,
            null); // Clear due date

        // Act
        var result = await handler.HandleAsync(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Null(result.DueDate);
    }
}
