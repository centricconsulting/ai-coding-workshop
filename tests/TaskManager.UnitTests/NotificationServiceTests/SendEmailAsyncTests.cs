using FakeItEasy;
using Microsoft.Extensions.Logging;
using TaskManager.Infrastructure.Services;

namespace TaskManager.UnitTests.NotificationServiceTests;

public sealed class SendEmailAsyncTests
{
    private readonly ILogger<NotificationService> _logger;
    private readonly NotificationService _sut;

    public SendEmailAsyncTests()
    {
        _logger = A.Fake<ILogger<NotificationService>>();
        _sut = new NotificationService(_logger);
    }

    [Fact]
    public async Task SendEmailAsync_WithValidParameters_SendsEmailSuccessfully()
    {
        // Arrange
        const string recipient = "test@example.com";
        const string subject = "Test Subject";
        const string message = "Test Message";

        // Act
        await _sut.SendEmailAsync(recipient, subject, message);

        // Assert
        // Verify no exceptions were thrown and method completed
        Assert.True(true);
    }

    [Fact]
    public async Task SendEmailAsync_WithNullRecipient_ThrowsArgumentException()
    {
        // Arrange
        const string subject = "Test Subject";
        const string message = "Test Message";

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => _sut.SendEmailAsync(null!, subject, message));
        Assert.Equal("recipient", exception.ParamName);
    }

    [Fact]
    public async Task SendEmailAsync_WithEmptyRecipient_ThrowsArgumentException()
    {
        // Arrange
        const string subject = "Test Subject";
        const string message = "Test Message";

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => _sut.SendEmailAsync(string.Empty, subject, message));
        Assert.Equal("recipient", exception.ParamName);
    }

    [Fact]
    public async Task SendEmailAsync_WithWhitespaceRecipient_ThrowsArgumentException()
    {
        // Arrange
        const string subject = "Test Subject";
        const string message = "Test Message";

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => _sut.SendEmailAsync("   ", subject, message));
        Assert.Equal("recipient", exception.ParamName);
    }

    [Fact]
    public async Task SendEmailAsync_WithNullSubject_ThrowsArgumentException()
    {
        // Arrange
        const string recipient = "test@example.com";
        const string message = "Test Message";

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => _sut.SendEmailAsync(recipient, null!, message));
        Assert.Equal("subject", exception.ParamName);
    }

    [Fact]
    public async Task SendEmailAsync_WithEmptySubject_ThrowsArgumentException()
    {
        // Arrange
        const string recipient = "test@example.com";
        const string message = "Test Message";

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => _sut.SendEmailAsync(recipient, string.Empty, message));
        Assert.Equal("subject", exception.ParamName);
    }

    [Fact]
    public async Task SendEmailAsync_WithWhitespaceSubject_ThrowsArgumentException()
    {
        // Arrange
        const string recipient = "test@example.com";
        const string message = "Test Message";

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => _sut.SendEmailAsync(recipient, "   ", message));
        Assert.Equal("subject", exception.ParamName);
    }

    [Fact]
    public async Task SendEmailAsync_WithNullMessage_ThrowsArgumentException()
    {
        // Arrange
        const string recipient = "test@example.com";
        const string subject = "Test Subject";

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => _sut.SendEmailAsync(recipient, subject, null!));
        Assert.Equal("message", exception.ParamName);
    }

    [Fact]
    public async Task SendEmailAsync_WithEmptyMessage_ThrowsArgumentException()
    {
        // Arrange
        const string recipient = "test@example.com";
        const string subject = "Test Subject";

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => _sut.SendEmailAsync(recipient, subject, string.Empty));
        Assert.Equal("message", exception.ParamName);
    }

    [Fact]
    public async Task SendEmailAsync_WithWhitespaceMessage_ThrowsArgumentException()
    {
        // Arrange
        const string recipient = "test@example.com";
        const string subject = "Test Subject";

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => _sut.SendEmailAsync(recipient, subject, "   "));
        Assert.Equal("message", exception.ParamName);
    }

    [Fact]
    public async Task SendEmailAsync_WithCancellationToken_CompletesSuccessfully()
    {
        // Arrange
        const string recipient = "test@example.com";
        const string subject = "Test Subject";
        const string message = "Test Message";
        using var cts = new CancellationTokenSource();

        // Act
        await _sut.SendEmailAsync(recipient, subject, message, cts.Token);

        // Assert
        Assert.True(true);
    }
}
