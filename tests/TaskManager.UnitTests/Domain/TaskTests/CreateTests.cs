namespace TaskManager.UnitTests.Domain.TaskTests;

using TaskManager.Domain.Tasks;
using TaskManager.Domain.ValueObjects;

public sealed class CreateTests
{
    [Fact]
    public void Create_WithValidTitleAndPriority_ShouldCreateTask()
    {
        // Arrange
        const string title = "Test Task";
        const string description = "Test Description";
        var priority = Priority.High;

        // Act
        var task = Task.Create(title, description, priority);

        // Assert
        Assert.NotNull(task);
        Assert.Equal(title, task.Title);
        Assert.Equal(description, task.Description);
        Assert.Equal(priority, task.Priority);
        Assert.Null(task.DueDate);
        Assert.Equal(TaskStatus.Todo, task.Status);
    }

    [Fact]
    public void Create_WithValidTitlePriorityAndDueDate_ShouldCreateTask()
    {
        // Arrange
        const string title = "Test Task";
        const string description = "Test Description";
        var priority = Priority.Medium;
        var dueDate = DateTime.UtcNow.AddDays(7);

        // Act
        var task = Task.Create(title, description, priority, dueDate);

        // Assert
        Assert.NotNull(task);
        Assert.Equal(title, task.Title);
        Assert.Equal(description, task.Description);
        Assert.Equal(priority, task.Priority);
        Assert.Equal(dueDate, task.DueDate);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithNullOrEmptyTitle_ShouldThrowArgumentException(string? invalidTitle)
    {
        // Arrange
        const string description = "Test Description";
        var priority = Priority.Low;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => 
            Task.Create(invalidTitle!, description, priority));
        Assert.Equal("title", exception.ParamName);
    }

    [Fact]
    public void Create_WithNullPriority_ShouldThrowArgumentNullException()
    {
        // Arrange
        const string title = "Test Task";
        const string description = "Test Description";

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => 
            Task.Create(title, description, null!));
        Assert.Equal("priority", exception.ParamName);
    }

    [Fact]
    public void Create_WithPastDueDate_ShouldThrowArgumentException()
    {
        // Arrange
        const string title = "Test Task";
        const string description = "Test Description";
        var priority = Priority.Critical;
        var pastDate = DateTime.UtcNow.AddDays(-1);

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => 
            Task.Create(title, description, priority, pastDate));
        Assert.Equal("dueDate", exception.ParamName);
        Assert.Contains("future", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Create_WithNullDueDate_ShouldCreateTaskWithoutDueDate()
    {
        // Arrange
        const string title = "Test Task";
        const string description = "Test Description";
        var priority = Priority.Low;

        // Act
        var task = Task.Create(title, description, priority, dueDate: null);

        // Assert
        Assert.NotNull(task);
        Assert.Null(task.DueDate);
    }
}
