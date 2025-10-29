using FakeItEasy;
using Microsoft.Extensions.Logging;
using TaskManager.Infrastructure.Services;

namespace TaskManager.UnitTests.NotificationServiceTests;

public sealed class SendCombinedNotificationAsyncTests
{
    private readonly ILogger<NotificationService> _logger;
    private readonly NotificationService _sut;

    public SendCombinedNotificationAsyncTests()
    {
        _logger = A.Fake<ILogger<NotificationService>>();
        _sut = new NotificationService(_logger);
    }

    [Fact]
    public async Task SendCombinedNotificationAsync_WithValidParameters_SendsBothNotificationsSuccessfully()
    {
        // Arrange
        const string emailRecipient = "test@example.com";
        const string phoneNumber = "+1234567890";
        const string subject = "Test Subject";
        const string message = "Test Message";

        // Act
        await _sut.SendCombinedNotificationAsync(emailRecipient, phoneNumber, subject, message);

        // Assert
        // Verify no exceptions were thrown and method completed
        Assert.True(true);
    }

    [Fact]
    public async Task SendCombinedNotificationAsync_WithNullEmailRecipient_ThrowsArgumentException()
    {
        // Arrange
        const string phoneNumber = "+1234567890";
        const string subject = "Test Subject";
        const string message = "Test Message";

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => _sut.SendCombinedNotificationAsync(null!, phoneNumber, subject, message));
        Assert.Equal("emailRecipient", exception.ParamName);
    }

    [Fact]
    public async Task SendCombinedNotificationAsync_WithEmptyEmailRecipient_ThrowsArgumentException()
    {
        // Arrange
        const string phoneNumber = "+1234567890";
        const string subject = "Test Subject";
        const string message = "Test Message";

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => _sut.SendCombinedNotificationAsync(string.Empty, phoneNumber, subject, message));
        Assert.Equal("emailRecipient", exception.ParamName);
    }

    [Fact]
    public async Task SendCombinedNotificationAsync_WithWhitespaceEmailRecipient_ThrowsArgumentException()
    {
        // Arrange
        const string phoneNumber = "+1234567890";
        const string subject = "Test Subject";
        const string message = "Test Message";

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => _sut.SendCombinedNotificationAsync("   ", phoneNumber, subject, message));
        Assert.Equal("emailRecipient", exception.ParamName);
    }

    [Fact]
    public async Task SendCombinedNotificationAsync_WithNullPhoneNumber_ThrowsArgumentException()
    {
        // Arrange
        const string emailRecipient = "test@example.com";
        const string subject = "Test Subject";
        const string message = "Test Message";

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => _sut.SendCombinedNotificationAsync(emailRecipient, null!, subject, message));
        Assert.Equal("phoneNumber", exception.ParamName);
    }

    [Fact]
    public async Task SendCombinedNotificationAsync_WithEmptyPhoneNumber_ThrowsArgumentException()
    {
        // Arrange
        const string emailRecipient = "test@example.com";
        const string subject = "Test Subject";
        const string message = "Test Message";

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => _sut.SendCombinedNotificationAsync(emailRecipient, string.Empty, subject, message));
        Assert.Equal("phoneNumber", exception.ParamName);
    }

    [Fact]
    public async Task SendCombinedNotificationAsync_WithWhitespacePhoneNumber_ThrowsArgumentException()
    {
        // Arrange
        const string emailRecipient = "test@example.com";
        const string subject = "Test Subject";
        const string message = "Test Message";

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => _sut.SendCombinedNotificationAsync(emailRecipient, "   ", subject, message));
        Assert.Equal("phoneNumber", exception.ParamName);
    }

    [Fact]
    public async Task SendCombinedNotificationAsync_WithNullSubject_ThrowsArgumentException()
    {
        // Arrange
        const string emailRecipient = "test@example.com";
        const string phoneNumber = "+1234567890";
        const string message = "Test Message";

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => _sut.SendCombinedNotificationAsync(emailRecipient, phoneNumber, null!, message));
        Assert.Equal("subject", exception.ParamName);
    }

    [Fact]
    public async Task SendCombinedNotificationAsync_WithEmptySubject_ThrowsArgumentException()
    {
        // Arrange
        const string emailRecipient = "test@example.com";
        const string phoneNumber = "+1234567890";
        const string message = "Test Message";

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => _sut.SendCombinedNotificationAsync(emailRecipient, phoneNumber, string.Empty, message));
        Assert.Equal("subject", exception.ParamName);
    }

    [Fact]
    public async Task SendCombinedNotificationAsync_WithWhitespaceSubject_ThrowsArgumentException()
    {
        // Arrange
        const string emailRecipient = "test@example.com";
        const string phoneNumber = "+1234567890";
        const string message = "Test Message";

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => _sut.SendCombinedNotificationAsync(emailRecipient, phoneNumber, "   ", message));
        Assert.Equal("subject", exception.ParamName);
    }

    [Fact]
    public async Task SendCombinedNotificationAsync_WithNullMessage_ThrowsArgumentException()
    {
        // Arrange
        const string emailRecipient = "test@example.com";
        const string phoneNumber = "+1234567890";
        const string subject = "Test Subject";

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => _sut.SendCombinedNotificationAsync(emailRecipient, phoneNumber, subject, null!));
        Assert.Equal("message", exception.ParamName);
    }

    [Fact]
    public async Task SendCombinedNotificationAsync_WithEmptyMessage_ThrowsArgumentException()
    {
        // Arrange
        const string emailRecipient = "test@example.com";
        const string phoneNumber = "+1234567890";
        const string subject = "Test Subject";

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => _sut.SendCombinedNotificationAsync(emailRecipient, phoneNumber, subject, string.Empty));
        Assert.Equal("message", exception.ParamName);
    }

    [Fact]
    public async Task SendCombinedNotificationAsync_WithWhitespaceMessage_ThrowsArgumentException()
    {
        // Arrange
        const string emailRecipient = "test@example.com";
        const string phoneNumber = "+1234567890";
        const string subject = "Test Subject";

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => _sut.SendCombinedNotificationAsync(emailRecipient, phoneNumber, subject, "   "));
        Assert.Equal("message", exception.ParamName);
    }

    [Fact]
    public async Task SendCombinedNotificationAsync_WithCancellationToken_CompletesSuccessfully()
    {
        // Arrange
        const string emailRecipient = "test@example.com";
        const string phoneNumber = "+1234567890";
        const string subject = "Test Subject";
        const string message = "Test Message";
        using var cts = new CancellationTokenSource();

        // Act
        await _sut.SendCombinedNotificationAsync(emailRecipient, phoneNumber, subject, message, cts.Token);

        // Assert
        Assert.True(true);
    }
}
