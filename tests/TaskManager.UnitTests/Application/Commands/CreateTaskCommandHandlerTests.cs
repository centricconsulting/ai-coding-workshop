namespace TaskManager.UnitTests.Application.Commands;

using FakeItEasy;
using Microsoft.Extensions.Logging;
using TaskManager.Application.Commands;
using TaskManager.Application.Handlers;
using TaskManager.Domain.Repositories;
using TaskManager.Domain.Tasks;
using TaskManager.Domain.ValueObjects;
using Xunit;
using DomainTask = TaskManager.Domain.Tasks.Task;
using DomainPriority = TaskManager.Domain.ValueObjects.Priority;
using Task = System.Threading.Tasks.Task;

/// <summary>
/// Tests for CreateTaskCommandHandler following TDD approach
/// Tests written FIRST (RED phase) before handler implementation
/// </summary>
public sealed class CreateTaskCommandHandlerTests
{
    private readonly ITaskRepository _taskRepository;
    private readonly ILogger<CreateTaskCommandHandler> _logger;
    private readonly CreateTaskCommandHandler _sut;

    public CreateTaskCommandHandlerTests()
    {
        _taskRepository = A.Fake<ITaskRepository>();
        _logger = A.Fake<ILogger<CreateTaskCommandHandler>>();
        _sut = new CreateTaskCommandHandler(_taskRepository, _logger);
    }

    [Fact]
    public async Task HandleAsync_WithValidCommand_CreatesTaskSuccessfully()
    {
        // Arrange
        var command = new CreateTaskCommand
        {
            Title = "Implement feature X",
            Description = "Add new functionality",
            Priority = "High",
            DueDate = DateTime.UtcNow.AddDays(7)
        };

        // Act
        var result = await _sut.HandleAsync(command);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(command.Title, result.Title);
        Assert.Equal(command.Description, result.Description);
        Assert.Equal(DomainPriority.High, result.Priority);
        Assert.Equal(command.DueDate, result.DueDate);
        Assert.Equal(TaskStatus.Todo, result.Status);
        
        A.CallTo(() => _taskRepository.AddTaskAsync(
            A<DomainTask>.That.Matches(t => 
                t.Title == command.Title && 
                t.Priority == DomainPriority.High),
            A<CancellationToken>._))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task HandleAsync_WithNullDueDate_CreatesTaskWithoutDueDate()
    {
        // Arrange
        var command = new CreateTaskCommand
        {
            Title = "Quick task",
            Description = "No deadline",
            Priority = "Low",
            DueDate = null
        };

        // Act
        var result = await _sut.HandleAsync(command);

        // Assert
        Assert.NotNull(result);
        Assert.Null(result.DueDate);
        Assert.Equal(DomainPriority.Low, result.Priority);
        
        A.CallTo(() => _taskRepository.AddTaskAsync(A<DomainTask>._, A<CancellationToken>._))
            .MustHaveHappenedOnceExactly();
    }

    [Theory]
    [InlineData("Low")]
    [InlineData("Medium")]
    [InlineData("High")]
    [InlineData("Critical")]
    public async Task HandleAsync_WithValidPriorityNames_ParsesCorrectly(string priorityName)
    {
        // Arrange
        var command = new CreateTaskCommand
        {
            Title = "Test task",
            Description = "Testing priority parsing",
            Priority = priorityName,
            DueDate = DateTime.UtcNow.AddDays(1)
        };

        // Act
        var result = await _sut.HandleAsync(command);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(priorityName, result.Priority.Name);
        
        A.CallTo(() => _taskRepository.AddTaskAsync(A<DomainTask>._, A<CancellationToken>._))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task HandleAsync_WithInvalidPriority_ThrowsArgumentException()
    {
        // Arrange
        var command = new CreateTaskCommand
        {
            Title = "Test task",
            Description = "Invalid priority",
            Priority = "SuperUrgent", // Invalid priority name
            DueDate = DateTime.UtcNow.AddDays(1)
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => 
            _sut.HandleAsync(command));
        
        Assert.Contains("priority", exception.Message, StringComparison.OrdinalIgnoreCase);
        
        A.CallTo(() => _taskRepository.AddTaskAsync(A<DomainTask>._, A<CancellationToken>._))
            .MustNotHaveHappened();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task HandleAsync_WithInvalidTitle_ThrowsArgumentException(string? invalidTitle)
    {
        // Arrange
        var command = new CreateTaskCommand
        {
            Title = invalidTitle!,
            Description = "Some description",
            Priority = "Medium",
            DueDate = DateTime.UtcNow.AddDays(1)
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => 
            _sut.HandleAsync(command));
        
        Assert.Contains("title", exception.Message, StringComparison.OrdinalIgnoreCase);
        
        A.CallTo(() => _taskRepository.AddTaskAsync(A<DomainTask>._, A<CancellationToken>._))
            .MustNotHaveHappened();
    }

    [Fact]
    public async Task HandleAsync_WithPastDueDate_ThrowsArgumentException()
    {
        // Arrange
        var command = new CreateTaskCommand
        {
            Title = "Past task",
            Description = "This due date is in the past",
            Priority = "High",
            DueDate = DateTime.UtcNow.AddDays(-1)
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => 
            _sut.HandleAsync(command));
        
        Assert.Contains("due date", exception.Message, StringComparison.OrdinalIgnoreCase);
        
        A.CallTo(() => _taskRepository.AddTaskAsync(A<DomainTask>._, A<CancellationToken>._))
            .MustNotHaveHappened();
    }

    [Fact]
    public async Task HandleAsync_WithNullCommand_ThrowsArgumentNullException()
    {
        // Arrange
        CreateTaskCommand? command = null;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => 
            _sut.HandleAsync(command!));
        
        A.CallTo(() => _taskRepository.AddTaskAsync(A<DomainTask>._, A<CancellationToken>._))
            .MustNotHaveHappened();
    }

    [Fact]
    public async Task HandleAsync_LogsTaskCreation()
    {
        // Arrange
        var command = new CreateTaskCommand
        {
            Title = "Logged task",
            Description = "Check logging",
            Priority = "Medium",
            DueDate = DateTime.UtcNow.AddDays(3)
        };

        // Act
        await _sut.HandleAsync(command);

        // Assert
        // Verify that logger was called with appropriate log level
        A.CallTo(_logger)
            .Where(call => call.Method.Name == "Log")
            .MustHaveHappened();
    }

    [Fact]
    public async Task HandleAsync_RespectsCancellationToken()
    {
        // Arrange
        var command = new CreateTaskCommand
        {
            Title = "Cancellable task",
            Description = "Should respect cancellation",
            Priority = "Low",
            DueDate = DateTime.UtcNow.AddDays(1)
        };
        
        var cts = new CancellationTokenSource();
        cts.Cancel();

        // Act & Assert
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => 
            _sut.HandleAsync(command, cts.Token));
    }
}
