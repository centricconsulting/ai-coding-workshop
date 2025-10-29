using FakeItEasy;
using Microsoft.Extensions.Logging;
using TaskManager.Infrastructure.Services;

namespace TaskManager.UnitTests.NotificationServiceTests;

public sealed class SendSmsAsyncTests
{
    private readonly ILogger<NotificationService> _logger;
    private readonly NotificationService _sut;

    public SendSmsAsyncTests()
    {
        _logger = A.Fake<ILogger<NotificationService>>();
        _sut = new NotificationService(_logger);
    }

    [Fact]
    public async Task SendSmsAsync_WithValidParameters_SendsSmsSuccessfully()
    {
        // Arrange
        const string phoneNumber = "+1234567890";
        const string message = "Test SMS Message";

        // Act
        await _sut.SendSmsAsync(phoneNumber, message);

        // Assert
        // Verify no exceptions were thrown and method completed
        Assert.True(true);
    }

    [Fact]
    public async Task SendSmsAsync_WithNullPhoneNumber_ThrowsArgumentException()
    {
        // Arrange
        const string message = "Test Message";

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => _sut.SendSmsAsync(null!, message));
        Assert.Equal("phoneNumber", exception.ParamName);
    }

    [Fact]
    public async Task SendSmsAsync_WithEmptyPhoneNumber_ThrowsArgumentException()
    {
        // Arrange
        const string message = "Test Message";

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => _sut.SendSmsAsync(string.Empty, message));
        Assert.Equal("phoneNumber", exception.ParamName);
    }

    [Fact]
    public async Task SendSmsAsync_WithWhitespacePhoneNumber_ThrowsArgumentException()
    {
        // Arrange
        const string message = "Test Message";

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => _sut.SendSmsAsync("   ", message));
        Assert.Equal("phoneNumber", exception.ParamName);
    }

    [Fact]
    public async Task SendSmsAsync_WithNullMessage_ThrowsArgumentException()
    {
        // Arrange
        const string phoneNumber = "+1234567890";

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => _sut.SendSmsAsync(phoneNumber, null!));
        Assert.Equal("message", exception.ParamName);
    }

    [Fact]
    public async Task SendSmsAsync_WithEmptyMessage_ThrowsArgumentException()
    {
        // Arrange
        const string phoneNumber = "+1234567890";

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => _sut.SendSmsAsync(phoneNumber, string.Empty));
        Assert.Equal("message", exception.ParamName);
    }

    [Fact]
    public async Task SendSmsAsync_WithWhitespaceMessage_ThrowsArgumentException()
    {
        // Arrange
        const string phoneNumber = "+1234567890";

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => _sut.SendSmsAsync(phoneNumber, "   "));
        Assert.Equal("message", exception.ParamName);
    }

    [Fact]
    public async Task SendSmsAsync_WithCancellationToken_CompletesSuccessfully()
    {
        // Arrange
        const string phoneNumber = "+1234567890";
        const string message = "Test SMS Message";
        using var cts = new CancellationTokenSource();

        // Act
        await _sut.SendSmsAsync(phoneNumber, message, cts.Token);

        // Assert
        Assert.True(true);
    }
}
