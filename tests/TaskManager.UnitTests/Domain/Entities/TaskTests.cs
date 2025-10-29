using DomainTask = TaskManager.Domain.Tasks.Task;
using DomainTaskStatus = TaskManager.Domain.Tasks.TaskStatus;
using TaskManager.Domain.Tasks;

namespace TaskManager.UnitTests.Domain.Entities;

/// <summary>
/// Tests for Task entity with priority and due date functionality
/// </summary>
public sealed class TaskTests
{
    [Fact]
    public void Create_WithValidTitleAndPriority_Succeeds()
    {
        // Arrange
        var title = "Test Task";
        var description = "Test Description";
        var priority = TaskPriority.High;

        // Act
        var task = DomainTask.Create(title, description, priority);

        // Assert
        Assert.NotNull(task);
        Assert.Equal(title, task.Title);
        Assert.Equal(description, task.Description);
        Assert.Equal(priority, task.Priority);
        Assert.Equal(DomainTaskStatus.Todo, task.Status);
    }

    [Fact]
    public void Create_WithValidTitlePriorityAndFutureDueDate_Succeeds()
    {
        // Arrange
        var title = "Task with Due Date";
        var description = "Important task";
        var priority = TaskPriority.Critical;
        var dueDate = DateTime.UtcNow.AddDays(7);

        // Act
        var task = DomainTask.Create(title, description, priority, dueDate);

        // Assert
        Assert.NotNull(task);
        Assert.Equal(title, task.Title);
        Assert.Equal(description, task.Description);
        Assert.Equal(priority, task.Priority);
        Assert.Equal(dueDate, task.DueDate);
    }

    [Fact]
    public void Create_WithNullTitle_ThrowsArgumentException()
    {
        // Arrange
        string? title = null;
        var description = "Test Description";
        var priority = TaskPriority.Medium;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => 
            DomainTask.Create(title!, description, priority));
        Assert.Contains("title", exception.Message.ToLower());
    }

    [Fact]
    public void Create_WithEmptyTitle_ThrowsArgumentException()
    {
        // Arrange
        var title = string.Empty;
        var description = "Test Description";
        var priority = TaskPriority.Medium;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => 
            DomainTask.Create(title, description, priority));
        Assert.Contains("title", exception.Message.ToLower());
    }

    [Fact]
    public void Create_WithWhitespaceTitle_ThrowsArgumentException()
    {
        // Arrange
        var title = "   ";
        var description = "Test Description";
        var priority = TaskPriority.Medium;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => 
            DomainTask.Create(title, description, priority));
        Assert.Contains("title", exception.Message.ToLower());
    }

    [Fact]
    public void Create_WithPastDueDate_ThrowsArgumentException()
    {
        // Arrange
        var title = "Past Task";
        var description = "Task with past due date";
        var priority = TaskPriority.High;
        var pastDueDate = DateTime.UtcNow.AddDays(-1);

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => 
            DomainTask.Create(title, description, priority, pastDueDate));
        Assert.Contains("past", exception.Message.ToLower());
    }

    [Fact]
    public void Create_WithNullDueDate_Succeeds()
    {
        // Arrange
        var title = "Task Without Due Date";
        var description = "No deadline";
        var priority = TaskPriority.Low;

        // Act
        var task = DomainTask.Create(title, description, priority, null);

        // Assert
        Assert.NotNull(task);
        Assert.Null(task.DueDate);
    }

    [Fact]
    public void Create_WithDefaultPriority_UsesMedium()
    {
        // Arrange
        var title = "Default Priority Task";
        var description = "Should have medium priority";

        // Act
        var task = DomainTask.Create(title, description);

        // Assert
        Assert.NotNull(task);
        Assert.Equal(TaskPriority.Medium, task.Priority);
    }

    [Fact]
    public void UpdatePriority_WithValidPriority_UpdatesSuccessfully()
    {
        // Arrange
        var task = DomainTask.Create("Test Task", "Description", TaskPriority.Low);
        var newPriority = TaskPriority.Critical;

        // Act
        task.UpdatePriority(newPriority);

        // Assert
        Assert.Equal(newPriority, task.Priority);
    }

    [Fact]
    public void UpdatePriority_WithNullPriority_ThrowsArgumentNullException()
    {
        // Arrange
        var task = DomainTask.Create("Test Task", "Description", TaskPriority.Medium);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => task.UpdatePriority(null!));
    }

    [Fact]
    public void UpdateDueDate_WithFutureDate_Succeeds()
    {
        // Arrange
        var task = DomainTask.Create("Test Task", "Description", TaskPriority.Medium);
        var futureDate = DateTime.UtcNow.AddDays(5);

        // Act
        task.UpdateDueDate(futureDate);

        // Assert
        Assert.Equal(futureDate, task.DueDate);
    }

    [Fact]
    public void UpdateDueDate_WithPastDate_ThrowsArgumentException()
    {
        // Arrange
        var task = DomainTask.Create("Test Task", "Description", TaskPriority.Medium);
        var pastDate = DateTime.UtcNow.AddDays(-1);

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => task.UpdateDueDate(pastDate));
        Assert.Contains("past", exception.Message.ToLower());
    }

    [Fact]
    public void UpdateDueDate_WithNull_ClearsDueDate()
    {
        // Arrange
        var task = DomainTask.Create("Test Task", "Description", TaskPriority.Medium, DateTime.UtcNow.AddDays(7));

        // Act
        task.UpdateDueDate(null);

        // Assert
        Assert.Null(task.DueDate);
    }

    [Fact]
    public void MarkAsCompleted_SetsIsCompletedToTrue()
    {
        // Arrange
        var task = DomainTask.Create("Test Task", "Description", TaskPriority.Medium);

        // Act
        task.MarkAsCompleted();

        // Assert
        Assert.True(task.IsCompleted);
    }

    [Fact]
    public void MarkAsCompleted_SetsCompletedAtToCurrentTime()
    {
        // Arrange
        var task = DomainTask.Create("Test Task", "Description", TaskPriority.Medium);
        var beforeCompletion = DateTime.UtcNow;

        // Act
        task.MarkAsCompleted();

        // Assert
        var afterCompletion = DateTime.UtcNow;
        Assert.NotNull(task.CompletedAt);
        Assert.True(task.CompletedAt >= beforeCompletion);
        Assert.True(task.CompletedAt <= afterCompletion);
    }

    [Fact]
    public void MarkAsCompleted_SetsStatusToDone()
    {
        // Arrange
        var task = DomainTask.Create("Test Task", "Description", TaskPriority.Medium);

        // Act
        task.MarkAsCompleted();

        // Assert
        Assert.Equal(DomainTaskStatus.Done, task.Status);
    }

    [Fact]
    public void IsCompleted_WhenStatusIsDone_ReturnsTrue()
    {
        // Arrange
        var task = DomainTask.Create("Test Task", "Description", TaskPriority.Medium);

        // Act
        task.MarkAsCompleted();

        // Assert
        Assert.True(task.IsCompleted);
        Assert.Equal(DomainTaskStatus.Done, task.Status);
    }

    [Fact]
    public void Create_WithLowPriority_Succeeds()
    {
        // Arrange
        var title = "Low Priority Task";
        var description = "Not urgent";
        var priority = TaskPriority.Low;

        // Act
        var task = DomainTask.Create(title, description, priority);

        // Assert
        Assert.Equal(priority, task.Priority);
    }

    [Fact]
    public void Create_GeneratesUniqueTaskId()
    {
        // Arrange & Act
        var task1 = DomainTask.Create("Task 1", "Description 1", TaskPriority.Medium);
        var task2 = DomainTask.Create("Task 2", "Description 2", TaskPriority.Medium);

        // Assert
        Assert.NotEqual(task1.Id, task2.Id);
    }
}
