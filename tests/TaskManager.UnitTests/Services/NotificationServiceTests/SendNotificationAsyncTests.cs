namespace TaskManager.UnitTests.Services.NotificationServiceTests;

using FakeItEasy;
using Microsoft.Extensions.Logging;
using TaskManager.Application.Services;
using Xunit;

public sealed class SendNotificationAsyncTests
{
    private readonly ILogger<NotificationService> _logger;
    private readonly NotificationService _sut;

    public SendNotificationAsyncTests()
    {
        _logger = A.Fake<ILogger<NotificationService>>();
        _sut = new NotificationService(_logger);
    }

    [Fact]
    public async Task SendNotificationAsync_WithValidInputs_SendsBothEmailAndSms()
    {
        // Arrange
        const string recipient = "user@example.com";
        const string phoneNumber = "+1234567890";
        const string subject = "Task Update";
        const string message = "Your task has been updated";

        // Act
        await _sut.SendNotificationAsync(recipient, phoneNumber, subject, message);

        // Assert
        // Verify logging occurred for combined notification
        A.CallTo(_logger).Where(call => 
            call.Method.Name == "Log" && 
            call.GetArgument<LogLevel>(0) == LogLevel.Information)
            .MustHaveHappened();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task SendNotificationAsync_WithInvalidRecipient_ThrowsArgumentException(string invalidRecipient)
    {
        // Arrange
        const string phoneNumber = "+1234567890";
        const string subject = "Test";
        const string message = "Test message";

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => 
            _sut.SendNotificationAsync(invalidRecipient, phoneNumber, subject, message));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task SendNotificationAsync_WithInvalidPhoneNumber_ThrowsArgumentException(string invalidPhoneNumber)
    {
        // Arrange
        const string recipient = "user@example.com";
        const string subject = "Test";
        const string message = "Test message";

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => 
            _sut.SendNotificationAsync(recipient, invalidPhoneNumber, subject, message));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task SendNotificationAsync_WithInvalidSubject_ThrowsArgumentException(string invalidSubject)
    {
        // Arrange
        const string recipient = "user@example.com";
        const string phoneNumber = "+1234567890";
        const string message = "Test message";

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => 
            _sut.SendNotificationAsync(recipient, phoneNumber, invalidSubject, message));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task SendNotificationAsync_WithInvalidMessage_ThrowsArgumentException(string invalidMessage)
    {
        // Arrange
        const string recipient = "user@example.com";
        const string phoneNumber = "+1234567890";
        const string subject = "Test";

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => 
            _sut.SendNotificationAsync(recipient, phoneNumber, subject, invalidMessage));
    }

    [Fact]
    public async Task SendNotificationAsync_RespectsCancellationToken()
    {
        // Arrange
        const string recipient = "user@example.com";
        const string phoneNumber = "+1234567890";
        const string subject = "Test";
        const string message = "Test message";
        var cts = new CancellationTokenSource();
        cts.Cancel();

        // Act & Assert
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => 
            _sut.SendNotificationAsync(recipient, phoneNumber, subject, message, cts.Token));
    }
}
