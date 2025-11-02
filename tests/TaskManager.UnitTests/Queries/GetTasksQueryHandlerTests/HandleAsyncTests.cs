using FakeItEasy;
using Microsoft.Extensions.Logging;
using TaskManager.Application.Queries;
using TaskManager.Domain.Repositories;
using TaskManager.Domain.Tasks;
using Xunit;
using DomainTask = TaskManager.Domain.Tasks.Task;
using DomainTaskStatus = TaskManager.Domain.Tasks.TaskStatus;
using TaskPriority = TaskManager.Domain.Tasks.TaskPriority;

namespace TaskManager.UnitTests.Queries.GetTasksQueryHandlerTests;

public class HandleAsyncTests
{
    private readonly ITaskRepository _fakeRepository;
    private readonly ILogger<GetTasksQueryHandler> _fakeLogger;
    private readonly GetTasksQueryHandler _handler;

    public HandleAsyncTests()
    {
        _fakeRepository = A.Fake<ITaskRepository>();
        _fakeLogger = A.Fake<ILogger<GetTasksQueryHandler>>();
        _handler = new GetTasksQueryHandler(_fakeRepository, _fakeLogger);
    }

    [Fact]
    public async System.Threading.Tasks.Task HandleAsync_WithNoStatusFilter_ReturnsAllTasksOrderedByCreatedAtDesc()
    {
        // Arrange
        var oldTask = DomainTask.Create("Old Task", "Description", TaskPriority.Low, null);
        await System.Threading.Tasks.Task.Delay(10); // Ensure different CreatedAt timestamps
        var middleTask = DomainTask.Create("Middle Task", "Description", TaskPriority.Medium, null);
        await System.Threading.Tasks.Task.Delay(10);
        var newTask = DomainTask.Create("New Task", "Description", TaskPriority.High, null);

        var tasks = new List<DomainTask> { oldTask, middleTask, newTask };
        A.CallTo(() => _fakeRepository.GetActiveTasksAsync(A<CancellationToken>._))
            .Returns(tasks);

        var query = new GetTasksQuery { Status = null };

        // Act
        var result = await _handler.HandleAsync(query);

        // Assert
        var resultList = result.ToList();
        Assert.Equal(3, resultList.Count);
        Assert.Equal(newTask.Id, resultList[0].Id); // Most recent first
        Assert.Equal(middleTask.Id, resultList[1].Id);
        Assert.Equal(oldTask.Id, resultList[2].Id); // Oldest last
    }

    [Fact]
    public async System.Threading.Tasks.Task HandleAsync_WithStatusFilter_ReturnsOnlyMatchingTasksOrderedDesc()
    {
        // Arrange
        var todoTask1 = DomainTask.Create("Todo 1", "Description", TaskPriority.Low, null);
        await System.Threading.Tasks.Task.Delay(10);
        var inProgressTask = DomainTask.Create("In Progress", "Description", TaskPriority.Medium, null);
        inProgressTask.UpdateStatus(DomainTaskStatus.InProgress);
        await System.Threading.Tasks.Task.Delay(10);
        var todoTask2 = DomainTask.Create("Todo 2", "Description", TaskPriority.High, null);

        var tasks = new List<DomainTask> { todoTask1, inProgressTask, todoTask2 };
        A.CallTo(() => _fakeRepository.GetActiveTasksAsync(A<CancellationToken>._))
            .Returns(tasks);

        var query = new GetTasksQuery { Status = DomainTaskStatus.Todo };

        // Act
        var result = await _handler.HandleAsync(query);

        // Assert
        var resultList = result.ToList();
        Assert.Equal(2, resultList.Count);
        Assert.All(resultList, t => Assert.Equal(DomainTaskStatus.Todo, t.Status));
        Assert.Equal(todoTask2.Id, resultList[0].Id); // Most recent Todo first
        Assert.Equal(todoTask1.Id, resultList[1].Id);
    }

    [Fact]
    public async System.Threading.Tasks.Task HandleAsync_WithStatusFilterInProgress_ReturnsOnlyInProgressTasks()
    {
        // Arrange
        var todoTask = DomainTask.Create("Todo", "Description", TaskPriority.Low, null);
        var inProgressTask1 = DomainTask.Create("In Progress 1", "Description", TaskPriority.Medium, null);
        inProgressTask1.UpdateStatus(DomainTaskStatus.InProgress);
        await System.Threading.Tasks.Task.Delay(10);
        var inProgressTask2 = DomainTask.Create("In Progress 2", "Description", TaskPriority.High, null);
        inProgressTask2.UpdateStatus(DomainTaskStatus.InProgress);

        var tasks = new List<DomainTask> { todoTask, inProgressTask1, inProgressTask2 };
        A.CallTo(() => _fakeRepository.GetActiveTasksAsync(A<CancellationToken>._))
            .Returns(tasks);

        var query = new GetTasksQuery { Status = DomainTaskStatus.InProgress };

        // Act
        var result = await _handler.HandleAsync(query);

        // Assert
        var resultList = result.ToList();
        Assert.Equal(2, resultList.Count);
        Assert.All(resultList, t => Assert.Equal(DomainTaskStatus.InProgress, t.Status));
    }

    [Fact]
    public async System.Threading.Tasks.Task HandleAsync_WithStatusFilterDone_ReturnsOnlyCompletedTasks()
    {
        // Arrange
        var todoTask = DomainTask.Create("Todo", "Description", TaskPriority.Low, null);
        var doneTask = DomainTask.Create("Done", "Description", TaskPriority.Medium, null);
        doneTask.UpdateStatus(DomainTaskStatus.Done);

        var tasks = new List<DomainTask> { todoTask, doneTask };
        A.CallTo(() => _fakeRepository.GetActiveTasksAsync(A<CancellationToken>._))
            .Returns(tasks);

        var query = new GetTasksQuery { Status = DomainTaskStatus.Done };

        // Act
        var result = await _handler.HandleAsync(query);

        // Assert
        var resultList = result.ToList();
        Assert.Single(resultList);
        Assert.Equal(DomainTaskStatus.Done, resultList[0].Status);
        Assert.Equal(doneTask.Id, resultList[0].Id);
    }

    [Fact]
    public async System.Threading.Tasks.Task HandleAsync_WhenNoTasksExist_ReturnsEmptyCollection()
    {
        // Arrange
        A.CallTo(() => _fakeRepository.GetActiveTasksAsync(A<CancellationToken>._))
            .Returns(Enumerable.Empty<DomainTask>());

        var query = new GetTasksQuery { Status = null };

        // Act
        var result = await _handler.HandleAsync(query);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async System.Threading.Tasks.Task HandleAsync_WhenNoTasksMatchFilter_ReturnsEmptyCollection()
    {
        // Arrange
        var todoTask = DomainTask.Create("Todo", "Description", TaskPriority.Low, null);
        var tasks = new List<DomainTask> { todoTask };
        A.CallTo(() => _fakeRepository.GetActiveTasksAsync(A<CancellationToken>._))
            .Returns(tasks);

        var query = new GetTasksQuery { Status = DomainTaskStatus.Done };

        // Act
        var result = await _handler.HandleAsync(query);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async System.Threading.Tasks.Task HandleAsync_WithNullQuery_ThrowsArgumentNullException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(
            async () => await _handler.HandleAsync(null!));
    }

    [Fact]
    public async System.Threading.Tasks.Task HandleAsync_PassesCancellationTokenToRepository()
    {
        // Arrange
        var cts = new CancellationTokenSource();
        var cancellationToken = cts.Token;
        A.CallTo(() => _fakeRepository.GetActiveTasksAsync(A<CancellationToken>._))
            .Returns(Enumerable.Empty<DomainTask>());

        var query = new GetTasksQuery { Status = null };

        // Act
        await _handler.HandleAsync(query, cancellationToken);

        // Assert
        A.CallTo(() => _fakeRepository.GetActiveTasksAsync(cancellationToken))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async System.Threading.Tasks.Task HandleAsync_LogsInformationAboutStatusFilter()
    {
        // Arrange
        A.CallTo(() => _fakeRepository.GetActiveTasksAsync(A<CancellationToken>._))
            .Returns(Enumerable.Empty<DomainTask>());

        var query = new GetTasksQuery { Status = DomainTaskStatus.InProgress };

        // Act
        await _handler.HandleAsync(query);

        // Assert - Logger was called (FakeItEasy captures calls)
        A.CallTo(_fakeLogger).Where(call => 
            call.Method.Name == "Log" && 
            call.GetArgument<LogLevel>(0) == LogLevel.Information)
            .MustHaveHappened();
    }

    [Fact]
    public async System.Threading.Tasks.Task HandleAsync_OrdersMultipleTasksByCreatedAtDescending()
    {
        // Arrange
        var task1 = DomainTask.Create("Task 1", "First", TaskPriority.Low, null);
        await System.Threading.Tasks.Task.Delay(10);
        var task2 = DomainTask.Create("Task 2", "Second", TaskPriority.Medium, null);
        await System.Threading.Tasks.Task.Delay(10);
        var task3 = DomainTask.Create("Task 3", "Third", TaskPriority.High, null);
        await System.Threading.Tasks.Task.Delay(10);
        var task4 = DomainTask.Create("Task 4", "Fourth", TaskPriority.Critical, null);

        var tasks = new List<DomainTask> { task1, task2, task3, task4 };
        A.CallTo(() => _fakeRepository.GetActiveTasksAsync(A<CancellationToken>._))
            .Returns(tasks);

        var query = new GetTasksQuery { Status = null };

        // Act
        var result = await _handler.HandleAsync(query);

        // Assert
        var resultList = result.ToList();
        Assert.Equal(4, resultList.Count);
        
        // Verify descending order by CreatedAt
        for (int i = 0; i < resultList.Count - 1; i++)
        {
            Assert.True(resultList[i].CreatedAt >= resultList[i + 1].CreatedAt,
                $"Task at index {i} should have CreatedAt >= task at index {i + 1}");
        }
        
        // Verify specific order
        Assert.Equal(task4.Id, resultList[0].Id);
        Assert.Equal(task3.Id, resultList[1].Id);
        Assert.Equal(task2.Id, resultList[2].Id);
        Assert.Equal(task1.Id, resultList[3].Id);
    }
}
