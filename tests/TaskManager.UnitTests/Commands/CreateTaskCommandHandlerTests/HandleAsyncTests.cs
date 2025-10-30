using FakeItEasy;
using Microsoft.Extensions.Logging;
using TaskManager.Application.Commands;
using TaskManager.Domain.Repositories;
using DomainTask = TaskManager.Domain.Tasks.Task;
using TaskPriority = TaskManager.Domain.Tasks.TaskPriority;

namespace TaskManager.UnitTests.Commands.CreateTaskCommandHandlerTests;

/// <summary>
/// Unit tests for CreateTaskCommandHandler.HandleAsync method
/// Tests follow TDD and CQRS patterns with comprehensive coverage
/// </summary>
public sealed class HandleAsyncTests
{
    private readonly ITaskRepository _repository;
    private readonly ILogger<CreateTaskCommandHandler> _logger;
    private readonly CreateTaskCommandHandler _sut;

    public HandleAsyncTests()
    {
        _repository = A.Fake<ITaskRepository>();
        _logger = A.Fake<ILogger<CreateTaskCommandHandler>>();
        _sut = new CreateTaskCommandHandler(_repository, _logger);
    }

    [Fact]
    public async Task  HandleAsync_WithValidCommand_CreatesTaskWithCorrectProperties()
    {
        // Arrange
        var command = new CreateTaskCommand
        {
            Title = "Test Task",
            Description = "Test Description",
            Priority = "High",
            DueDate = DateTime.UtcNow.AddDays(7)
        };

        DomainTask? capturedTask = null;
        A.CallTo(() => _repository.AddTaskAsync(A<DomainTask>._, A<CancellationToken>._))
            .Invokes((DomainTask task, CancellationToken _) => capturedTask = task);

        // Act
        var result = await _sut.HandleAsync(command);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(capturedTask);
        Assert.Equal(command.Title, capturedTask.Title);
        Assert.Equal(command.Description, capturedTask.Description);
        Assert.Equal(TaskPriority.High, capturedTask.Priority);
        Assert.Equal(command.DueDate, capturedTask.DueDate);
        A.CallTo(() => _repository.AddTaskAsync(A<DomainTask>._, A<CancellationToken>._))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task HandleAsync_WithValidCommandNoDescription_CreatesTask()
    {
        // Arrange
        var command = new CreateTaskCommand
        {
            Title = "Task Without Description",
            Description = null,
            Priority = "Medium",
            DueDate = null
        };

        // Act
        var result = await _sut.HandleAsync(command);

        // Assert
        Assert.NotNull(result);
        A.CallTo(() => _repository.AddTaskAsync(A<DomainTask>._, A<CancellationToken>._))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task HandleAsync_WithLowPriority_CreatesTaskWithLowPriority()
    {
        // Arrange
        var command = new CreateTaskCommand
        {
            Title = "Low Priority Task",
            Description = "Not urgent",
            Priority = "Low"
        };

        DomainTask? capturedTask = null;
        A.CallTo(() => _repository.AddTaskAsync(A<DomainTask>._, A<CancellationToken>._))
            .Invokes((DomainTask task, CancellationToken _) => capturedTask = task);

        // Act
        var result = await _sut.HandleAsync(command);

        // Assert
        Assert.NotNull(capturedTask);
        Assert.Equal(TaskPriority.Low, capturedTask.Priority);
    }

    [Fact]
    public async Task HandleAsync_WithCriticalPriority_CreatesTaskWithCriticalPriority()
    {
        // Arrange
        var command = new CreateTaskCommand
        {
            Title = "Critical Task",
            Description = "Very urgent",
            Priority = "Critical"
        };

        DomainTask? capturedTask = null;
        A.CallTo(() => _repository.AddTaskAsync(A<DomainTask>._, A<CancellationToken>._))
            .Invokes((DomainTask task, CancellationToken _) => capturedTask = task);

        // Act
        var result = await _sut.HandleAsync(command);

        // Assert
        Assert.NotNull(capturedTask);
        Assert.Equal(TaskPriority.Critical, capturedTask.Priority);
    }

    [Fact]
    public async Task HandleAsync_WithInvalidPriorityString_ThrowsArgumentException()
    {
        // Arrange
        var command = new CreateTaskCommand
        {
            Title = "Test Task",
            Description = "Test Description",
            Priority = "InvalidPriority",
            DueDate = null
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            async () => await _sut.HandleAsync(command));
        Assert.Contains("priority", exception.Message.ToLower());
    }

    [Fact]
    public async Task HandleAsync_WithEmptyPriorityString_ThrowsArgumentException()
    {
        // Arrange
        var command = new CreateTaskCommand
        {
            Title = "Test Task",
            Description = "Test Description",
            Priority = "",
            DueDate = null
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(
            async () => await _sut.HandleAsync(command));
    }

    [Fact]
    public async Task HandleAsync_WithPastDueDate_ThrowsArgumentException()
    {
        // Arrange
        var command = new CreateTaskCommand
        {
            Title = "Test Task",
            Description = "Test Description",
            Priority = "High",
            DueDate = DateTime.UtcNow.AddDays(-1)
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            async () => await _sut.HandleAsync(command));
        Assert.Contains("past", exception.Message.ToLower());
    }

    [Fact]
    public async Task HandleAsync_WithNullTitle_ThrowsArgumentException()
    {
        // Arrange
        var command = new CreateTaskCommand
        {
            Title = null!,
            Description = "Test Description",
            Priority = "Medium"
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(
            async () => await _sut.HandleAsync(command));
    }

    [Fact]
    public async Task HandleAsync_WithEmptyTitle_ThrowsArgumentException()
    {
        // Arrange
        var command = new CreateTaskCommand
        {
            Title = "",
            Description = "Test Description",
            Priority = "Medium"
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(
            async () => await _sut.HandleAsync(command));
    }

    [Fact]
    public async Task HandleAsync_WithWhitespaceTitle_ThrowsArgumentException()
    {
        // Arrange
        var command = new CreateTaskCommand
        {
            Title = "   ",
            Description = "Test Description",
            Priority = "Medium"
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(
            async () => await _sut.HandleAsync(command));
    }

    [Fact]
    public async Task HandleAsync_WithNullCommand_ThrowsArgumentNullException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(
            async () => await _sut.HandleAsync(null!));
    }

    [Fact]
    public async Task HandleAsync_WithValidCommand_LogsInformation()
    {
        // Arrange
        var command = new CreateTaskCommand
        {
            Title = "Test Task",
            Description = "Test Description",
            Priority = "High"
        };

        // Act
        await _sut.HandleAsync(command);

        // Assert
        A.CallTo(_logger)
            .Where(call => call.Method.Name == "Log")
            .MustHaveHappened();
    }

    [Fact]
    public async Task HandleAsync_WithValidCommand_ReturnsTaskWithGeneratedId()
    {
        // Arrange
        var command = new CreateTaskCommand
        {
            Title = "Test Task",
            Description = "Test Description",
            Priority = "Medium"
        };

        // Act
        var result = await _sut.HandleAsync(command);

        // Assert
        Assert.NotNull(result);
        Assert.NotEqual(Guid.Empty, result.Id.Value);
    }

    [Fact]
    public async Task HandleAsync_WithFutureDueDate_CreatesTaskSuccessfully()
    {
        // Arrange
        var futureDate = DateTime.UtcNow.AddMonths(1);
        var command = new CreateTaskCommand
        {
            Title = "Future Task",
            Description = "Task with future due date",
            Priority = "High",
            DueDate = futureDate
        };

        DomainTask? capturedTask = null;
        A.CallTo(() => _repository.AddTaskAsync(A<DomainTask>._, A<CancellationToken>._))
            .Invokes((DomainTask task, CancellationToken _) => capturedTask = task);

        // Act
        var result = await _sut.HandleAsync(command);

        // Assert
        Assert.NotNull(capturedTask);
        Assert.Equal(futureDate, capturedTask.DueDate);
    }

    [Fact]
    public async Task HandleAsync_WithNullDueDate_CreatesTaskWithoutDueDate()
    {
        // Arrange
        var command = new CreateTaskCommand
        {
            Title = "Task Without Due Date",
            Description = "No deadline",
            Priority = "Low",
            DueDate = null
        };

        DomainTask? capturedTask = null;
        A.CallTo(() => _repository.AddTaskAsync(A<DomainTask>._, A<CancellationToken>._))
            .Invokes((DomainTask task, CancellationToken _) => capturedTask = task);

        // Act
        var result = await _sut.HandleAsync(command);

        // Assert
        Assert.NotNull(capturedTask);
        Assert.Null(capturedTask.DueDate);
    }

    [Fact]
    public async Task HandleAsync_WhenRepositoryThrows_PropagatesException()
    {
        // Arrange
        var command = new CreateTaskCommand
        {
            Title = "Test Task",
            Description = "Test Description",
            Priority = "High"
        };

        A.CallTo(() => _repository.AddTaskAsync(A<DomainTask>._, A<CancellationToken>._))
            .Throws<InvalidOperationException>();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(
            async () => await _sut.HandleAsync(command));
    }

    [Fact]
    public async Task HandleAsync_WithCancellationToken_PassesTokenToRepository()
    {
        // Arrange
        var command = new CreateTaskCommand
        {
            Title = "Test Task",
            Description = "Test Description",
            Priority = "Medium"
        };
        var cancellationToken = new CancellationToken();

        // Act
        await _sut.HandleAsync(command, cancellationToken);

        // Assert
        A.CallTo(() => _repository.AddTaskAsync(A<DomainTask>._, cancellationToken))
            .MustHaveHappenedOnceExactly();
    }
}
