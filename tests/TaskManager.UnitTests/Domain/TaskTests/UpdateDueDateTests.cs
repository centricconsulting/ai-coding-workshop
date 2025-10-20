namespace TaskManager.UnitTests.Domain.TaskTests;

using TaskManager.Domain.Tasks;
using TaskManager.Domain.ValueObjects;

public sealed class UpdateDueDateTests
{
    [Fact]
    public void UpdateDueDate_WithValidFutureDate_ShouldUpdateDueDate()
    {
        // Arrange
        var task = Task.Create("Test Task", "Description", Priority.Low);
        var newDueDate = DateTime.UtcNow.AddDays(5);

        // Act
        task.UpdateDueDate(newDueDate);

        // Assert
        Assert.Equal(newDueDate, task.DueDate);
    }

    [Fact]
    public void UpdateDueDate_WithPastDate_ShouldThrowArgumentException()
    {
        // Arrange
        var task = Task.Create("Test Task", "Description", Priority.High);
        var pastDate = DateTime.UtcNow.AddDays(-1);

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => 
            task.UpdateDueDate(pastDate));
        Assert.Equal("dueDate", exception.ParamName);
        Assert.Contains("future", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void UpdateDueDate_WithNull_ShouldClearDueDate()
    {
        // Arrange
        var task = Task.Create("Test Task", "Description", Priority.Medium, DateTime.UtcNow.AddDays(3));

        // Act
        task.UpdateDueDate(null);

        // Assert
        Assert.Null(task.DueDate);
    }
}
