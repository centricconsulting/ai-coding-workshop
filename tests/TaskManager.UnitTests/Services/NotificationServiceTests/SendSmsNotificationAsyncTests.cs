namespace TaskManager.UnitTests.Services.NotificationServiceTests;

using FakeItEasy;
using Microsoft.Extensions.Logging;
using TaskManager.Application.Services;
using Xunit;

public sealed class SendSmsNotificationAsyncTests
{
    private readonly ILogger<NotificationService> _logger;
    private readonly NotificationService _sut;

    public SendSmsNotificationAsyncTests()
    {
        _logger = A.Fake<ILogger<NotificationService>>();
        _sut = new NotificationService(_logger);
    }

    [Fact]
    public async Task SendSmsNotificationAsync_WithValidInputs_SendsSms()
    {
        // Arrange
        const string phoneNumber = "+1234567890";
        const string message = "Your task has been updated";

        // Act
        await _sut.SendSmsNotificationAsync(phoneNumber, message);

        // Assert
        // Verify logging occurred
        A.CallTo(_logger).Where(call => 
            call.Method.Name == "Log" && 
            call.GetArgument<LogLevel>(0) == LogLevel.Information)
            .MustHaveHappened();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task SendSmsNotificationAsync_WithInvalidPhoneNumber_ThrowsArgumentException(string invalidPhoneNumber)
    {
        // Arrange
        const string message = "Test message";

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => 
            _sut.SendSmsNotificationAsync(invalidPhoneNumber, message));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task SendSmsNotificationAsync_WithInvalidMessage_ThrowsArgumentException(string invalidMessage)
    {
        // Arrange
        const string phoneNumber = "+1234567890";

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => 
            _sut.SendSmsNotificationAsync(phoneNumber, invalidMessage));
    }

    [Fact]
    public async Task SendSmsNotificationAsync_RespectsCancellationToken()
    {
        // Arrange
        const string phoneNumber = "+1234567890";
        const string message = "Test message";
        var cts = new CancellationTokenSource();
        cts.Cancel();

        // Act & Assert
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => 
            _sut.SendSmsNotificationAsync(phoneNumber, message, cts.Token));
    }
}
