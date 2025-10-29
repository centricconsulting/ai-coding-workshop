using FakeItEasy;
using Microsoft.Extensions.Logging;
using TaskManager.Infrastructure.Services;

namespace TaskManager.UnitTests.NotificationServiceTests;

public sealed class ConstructorTests
{
    [Fact]
    public void Constructor_WithValidLogger_CreatesInstance()
    {
        // Arrange
        var logger = A.Fake<ILogger<NotificationService>>();

        // Act
        var sut = new NotificationService(logger);

        // Assert
        Assert.NotNull(sut);
    }

    [Fact]
    public void Constructor_WithNullLogger_ThrowsArgumentNullException()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(
            () => new NotificationService(null!));
        Assert.Equal("logger", exception.ParamName);
    }
}
