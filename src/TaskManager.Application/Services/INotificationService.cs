namespace TaskManager.Application.Services;

/// <summary>
/// Service for sending notifications about task changes and updates.
/// </summary>
public interface INotificationService
{
    /// <summary>
    /// Sends an email notification about a task.
    /// </summary>
    /// <param name="recipient">The email address of the recipient.</param>
    /// <param name="subject">The email subject.</param>
    /// <param name="message">The email message body.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SendEmailAsync(
        string recipient,
        string subject,
        string message,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends an SMS notification about a task.
    /// </summary>
    /// <param name="phoneNumber">The recipient's phone number.</param>
    /// <param name="message">The SMS message content.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SendSmsAsync(
        string phoneNumber,
        string message,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends both email and SMS notifications about a task.
    /// </summary>
    /// <param name="emailRecipient">The email address of the recipient.</param>
    /// <param name="phoneNumber">The recipient's phone number.</param>
    /// <param name="subject">The email subject.</param>
    /// <param name="message">The notification message content.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SendCombinedNotificationAsync(
        string emailRecipient,
        string phoneNumber,
        string subject,
        string message,
        CancellationToken cancellationToken = default);
}
