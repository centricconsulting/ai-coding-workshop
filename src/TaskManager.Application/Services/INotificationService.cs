namespace TaskManager.Application.Services;

/// <summary>
/// Service for sending task notifications via email and SMS.
/// </summary>
public interface INotificationService
{
    /// <summary>
    /// Sends an email notification about a task.
    /// </summary>
    /// <param name="recipient">The email address of the recipient.</param>
    /// <param name="subject">The email subject line.</param>
    /// <param name="message">The notification message content.</param>
    /// <param name="cancellationToken">Cancellation token for the async operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SendEmailNotificationAsync(
        string recipient, 
        string subject, 
        string message, 
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Sends an SMS notification about a task.
    /// </summary>
    /// <param name="phoneNumber">The recipient's phone number.</param>
    /// <param name="message">The notification message content.</param>
    /// <param name="cancellationToken">Cancellation token for the async operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SendSmsNotificationAsync(
        string phoneNumber, 
        string message, 
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Sends both email and SMS notifications about a task.
    /// </summary>
    /// <param name="recipient">The email address of the recipient.</param>
    /// <param name="phoneNumber">The recipient's phone number.</param>
    /// <param name="subject">The email subject line.</param>
    /// <param name="message">The notification message content.</param>
    /// <param name="cancellationToken">Cancellation token for the async operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SendNotificationAsync(
        string recipient, 
        string phoneNumber, 
        string subject, 
        string message, 
        CancellationToken cancellationToken = default);
}
