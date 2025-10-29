using Microsoft.Extensions.Logging;
using TaskManager.Application.Services;

namespace TaskManager.Infrastructure.Services;

/// <summary>
/// Implementation of notification service for sending email and SMS notifications.
/// </summary>
public sealed class NotificationService : INotificationService
{
    private readonly ILogger<NotificationService> _logger;

    public NotificationService(ILogger<NotificationService> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task SendEmailAsync(
        string recipient,
        string subject,
        string message,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(recipient))
            throw new ArgumentException("Recipient cannot be null or empty", nameof(recipient));

        if (string.IsNullOrWhiteSpace(subject))
            throw new ArgumentException("Subject cannot be null or empty", nameof(subject));

        if (string.IsNullOrWhiteSpace(message))
            throw new ArgumentException("Message cannot be null or empty", nameof(message));

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

    public async Task SendSmsAsync(
        string phoneNumber,
        string message,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            throw new ArgumentException("Phone number cannot be null or empty", nameof(phoneNumber));

        if (string.IsNullOrWhiteSpace(message))
            throw new ArgumentException("Message cannot be null or empty", nameof(message));

        _logger.LogInformation(
            "Sending SMS notification to {PhoneNumber}",
            phoneNumber);

        // Simulate SMS sending
        await Task.Delay(100, cancellationToken);

        _logger.LogInformation(
            "SMS notification sent successfully to {PhoneNumber}",
            phoneNumber);
    }

    public async Task SendCombinedNotificationAsync(
        string emailRecipient,
        string phoneNumber,
        string subject,
        string message,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(emailRecipient))
            throw new ArgumentException("Recipient cannot be null or empty", nameof(emailRecipient));

        if (string.IsNullOrWhiteSpace(phoneNumber))
            throw new ArgumentException("Phone number cannot be null or empty", nameof(phoneNumber));

        if (string.IsNullOrWhiteSpace(subject))
            throw new ArgumentException("Subject cannot be null or empty", nameof(subject));

        if (string.IsNullOrWhiteSpace(message))
            throw new ArgumentException("Message cannot be null or empty", nameof(message));

        _logger.LogInformation(
            "Sending combined notification to email {Recipient} and phone {PhoneNumber}",
            emailRecipient,
            phoneNumber);

        await SendEmailAsync(emailRecipient, subject, message, cancellationToken);
        await SendSmsAsync(phoneNumber, message, cancellationToken);

        _logger.LogInformation(
            "Combined notification sent successfully");
    }
}
