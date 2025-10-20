namespace TaskManager.UnitTests.Domain.TaskTests;

using TaskManager.Domain.Tasks;
using TaskManager.Domain.ValueObjects;

public sealed class UpdatePriorityTests
{
    [Fact]
    public void UpdatePriority_WithValidPriority_ShouldUpdatePriority()
    {
        // Arrange
        var task = Task.Create("Test Task", "Description", Priority.Low);
        var newPriority = Priority.Critical;

        // Act
        task.UpdatePriority(newPriority);

        // Assert
        Assert.Equal(newPriority, task.Priority);
    }

    [Fact]
    public void UpdatePriority_WithNullPriority_ShouldThrowArgumentNullException()
    {
        // Arrange
        var task = Task.Create("Test Task", "Description", Priority.Medium);

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => 
            task.UpdatePriority(null!));
        Assert.Equal("priority", exception.ParamName);
    }
}
