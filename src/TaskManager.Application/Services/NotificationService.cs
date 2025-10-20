namespace TaskManager.Application.Services;

using Microsoft.Extensions.Logging;

/// <summary>
/// Service for sending task notifications via email and SMS.
/// Provides methods for individual and combined notification delivery.
/// </summary>
public sealed class NotificationService : INotificationService
{
    private readonly ILogger<NotificationService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="NotificationService"/> class.
    /// </summary>
    /// <param name="logger">The logger for structured logging.</param>
    /// <exception cref="ArgumentNullException">Thrown when logger is null.</exception>
    public NotificationService(ILogger<NotificationService> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task SendEmailNotificationAsync(
        string recipient,
        string subject,
        string message,
        CancellationToken cancellationToken = default)
    {
        ValidateNotNullOrWhitespace(recipient, nameof(recipient));
        ValidateNotNullOrWhitespace(subject, nameof(subject));
        ValidateNotNullOrWhitespace(message, nameof(message));

        _logger.LogInformation(
            "Sending email notification to {Recipient} with subject {Subject}",
            recipient,
            subject);

        // Simulate email sending
        await Task.Delay(100, cancellationToken);

        _logger.LogInformation(
            "Email notification sent successfully to {Recipient}",
            recipient);
    }

    /// <inheritdoc />
    public async Task SendSmsNotificationAsync(
        string phoneNumber,
        string message,
        CancellationToken cancellationToken = default)
    {
        ValidateNotNullOrWhitespace(phoneNumber, nameof(phoneNumber));
        ValidateNotNullOrWhitespace(message, nameof(message));

        _logger.LogInformation(
            "Sending SMS notification to {PhoneNumber}",
            phoneNumber);

        // Simulate SMS sending
        await Task.Delay(100, cancellationToken);

        _logger.LogInformation(
            "SMS notification sent successfully to {PhoneNumber}",
            phoneNumber);
    }

    /// <inheritdoc />
    public async Task SendNotificationAsync(
        string recipient,
        string phoneNumber,
        string subject,
        string message,
        CancellationToken cancellationToken = default)
    {
        ValidateNotNullOrWhitespace(recipient, nameof(recipient));
        ValidateNotNullOrWhitespace(phoneNumber, nameof(phoneNumber));
        ValidateNotNullOrWhitespace(subject, nameof(subject));
        ValidateNotNullOrWhitespace(message, nameof(message));

        _logger.LogInformation(
            "Sending combined notification to email {Recipient} and phone {PhoneNumber}",
            recipient,
            phoneNumber);

        await SendEmailNotificationAsync(recipient, subject, message, cancellationToken);
        await SendSmsNotificationAsync(phoneNumber, message, cancellationToken);

        _logger.LogInformation(
            "Combined notification sent successfully");
    }

    /// <summary>
    /// Validates that a string parameter is not null, empty, or whitespace.
    /// </summary>
    /// <param name="value">The value to validate.</param>
    /// <param name="parameterName">The name of the parameter being validated.</param>
    /// <exception cref="ArgumentException">Thrown when value is null, empty, or whitespace.</exception>
    private static void ValidateNotNullOrWhitespace(string value, string parameterName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"{parameterName} cannot be null or empty", parameterName);
    }
}
