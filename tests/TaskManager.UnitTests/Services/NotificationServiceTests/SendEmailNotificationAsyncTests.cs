namespace TaskManager.UnitTests.Services.NotificationServiceTests;

using FakeItEasy;
using Microsoft.Extensions.Logging;
using TaskManager.Application.Services;
using Xunit;

public sealed class SendEmailNotificationAsyncTests
{
    private readonly ILogger<NotificationService> _logger;
    private readonly NotificationService _sut;

    public SendEmailNotificationAsyncTests()
    {
        _logger = A.Fake<ILogger<NotificationService>>();
        _sut = new NotificationService(_logger);
    }

    [Fact]
    public async Task SendEmailNotificationAsync_WithValidInputs_SendsEmail()
    {
        // Arrange
        const string recipient = "user@example.com";
        const string subject = "Task Update";
        const string message = "Your task has been updated";

        // Act
        await _sut.SendEmailNotificationAsync(recipient, subject, message);

        // Assert
        // Verify logging occurred (implementation detail we'll check)
        A.CallTo(_logger).Where(call => 
            call.Method.Name == "Log" && 
            call.GetArgument<LogLevel>(0) == LogLevel.Information)
            .MustHaveHappened();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task SendEmailNotificationAsync_WithInvalidRecipient_ThrowsArgumentException(string invalidRecipient)
    {
        // Arrange
        const string subject = "Test";
        const string message = "Test message";

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => 
            _sut.SendEmailNotificationAsync(invalidRecipient, subject, message));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task SendEmailNotificationAsync_WithInvalidSubject_ThrowsArgumentException(string invalidSubject)
    {
        // Arrange
        const string recipient = "user@example.com";
        const string message = "Test message";

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => 
            _sut.SendEmailNotificationAsync(recipient, invalidSubject, message));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task SendEmailNotificationAsync_WithInvalidMessage_ThrowsArgumentException(string invalidMessage)
    {
        // Arrange
        const string recipient = "user@example.com";
        const string subject = "Test";

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => 
            _sut.SendEmailNotificationAsync(recipient, subject, invalidMessage));
    }

    [Fact]
    public async Task SendEmailNotificationAsync_RespectsCancellationToken()
    {
        // Arrange
        const string recipient = "user@example.com";
        const string subject = "Test";
        const string message = "Test message";
        var cts = new CancellationTokenSource();
        cts.Cancel();

        // Act & Assert
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => 
            _sut.SendEmailNotificationAsync(recipient, subject, message, cts.Token));
    }
}
