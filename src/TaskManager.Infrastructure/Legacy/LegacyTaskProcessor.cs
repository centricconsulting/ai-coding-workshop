using System.Text;
using Microsoft.Extensions.Logging;

namespace TaskManager.Infrastructure.Legacy;

/// <summary>
/// Interface for writing task output to external storage
/// Follows Single Responsibility Principle and Dependency Inversion
/// </summary>
public interface ITaskOutputWriter
{
    /// <summary>
    /// Writes task output asynchronously
    /// </summary>
    Task WriteTaskOutputAsync(int taskId, string content, CancellationToken cancellationToken = default);
}

/// <summary>
/// Defines the processing type for task text transformation
/// </summary>
public enum TextProcessingType
{
    /// <summary>
    /// Toggle case and replace spaces with underscores (with advanced processing)
    /// </summary>
    ToggleCaseWithUnderscores = 1,
    
    /// <summary>
    /// Sentence case: first word as-is, remaining words lowercase
    /// </summary>
    SentenceCase = 2,
    
    /// <summary>
    /// No transformation
    /// </summary>
    None = 3
}

/// <summary>
/// Refactored task processor following Clean Code principles
/// - Uses guard clauses (fail fast)
/// - Async/await pattern
/// - Structured logging
/// - Single Responsibility Principle
/// - Proper error handling
/// - Separated concerns (file I/O extracted)
/// - Object Calisthenics (max 2 indentation levels)
/// </summary>
public sealed class LegacyTaskProcessor
{
    private const int MaxOutputLength = 50;
    private const int ProcessingDelayMilliseconds = 100;
    
    private readonly ITaskOutputWriter _outputWriter;
    private readonly ILogger<LegacyTaskProcessor> _logger;

    public LegacyTaskProcessor(
        ITaskOutputWriter outputWriter,
        ILogger<LegacyTaskProcessor> logger)
    {
        _outputWriter = outputWriter ?? throw new ArgumentNullException(nameof(outputWriter));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Processes task text based on the specified processing type
    /// </summary>
    /// <param name="taskId">The unique identifier for the task</param>
    /// <param name="inputText">The text to process</param>
    /// <param name="processingType">The type of text transformation to apply</param>
    /// <param name="enableAdvancedProcessing">Whether to enable advanced processing (file output and delays)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The processed text</returns>
    public async Task<string> ProcessTaskAsync(
        int taskId,
        string inputText,
        TextProcessingType processingType,
        bool enableAdvancedProcessing,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "Processing task {TaskId} with type {ProcessingType}, advanced: {AdvancedProcessing}",
            taskId,
            processingType,
            enableAdvancedProcessing);

        // Guard clauses: fail fast
        if (string.IsNullOrEmpty(inputText))
        {
            _logger.LogWarning("Task {TaskId} has null or empty input text", taskId);
            return string.Empty;
        }

        try
        {
            var processedText = processingType switch
            {
                TextProcessingType.ToggleCaseWithUnderscores => 
                    ProcessToggleCaseWithUnderscores(inputText, enableAdvancedProcessing),
                
                TextProcessingType.SentenceCase => 
                    ProcessSentenceCase(inputText),
                
                TextProcessingType.None => 
                    inputText,
                
                _ => throw new ArgumentOutOfRangeException(
                    nameof(processingType), 
                    processingType, 
                    $"Unsupported processing type: {processingType}")
            };

            if (enableAdvancedProcessing)
            {
                await PerformAdvancedProcessingAsync(taskId, processedText, cancellationToken);
            }

            _logger.LogInformation(
                "Successfully processed task {TaskId}, output length: {Length}",
                taskId,
                processedText.Length);

            return processedText;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to process task {TaskId} with type {ProcessingType}",
                taskId,
                processingType);
            throw;
        }
    }

    /// <summary>
    /// Toggles character case and replaces spaces with underscores
    /// </summary>
    private string ProcessToggleCaseWithUnderscores(string inputText, bool enableAdvancedProcessing)
    {
        if (enableAdvancedProcessing)
        {
            return ApplyToggleCaseWithUnderscoresAndTruncate(inputText);
        }

        return inputText.ToUpperInvariant();
    }

    /// <summary>
    /// Applies toggle case transformation with underscore replacement and truncation
    /// </summary>
    private string ApplyToggleCaseWithUnderscoresAndTruncate(string inputText)
    {
        var builder = new StringBuilder(inputText.Length);

        foreach (var character in inputText)
        {
            if (character == ' ')
            {
                builder.Append('_');
                continue;
            }

            var transformedChar = char.IsUpper(character) 
                ? char.ToLowerInvariant(character) 
                : char.ToUpperInvariant(character);
            
            builder.Append(transformedChar);
        }

        var result = builder.ToString();
        return TruncateToMaxLength(result);
    }

    /// <summary>
    /// Processes text to sentence case: first word as-is, remaining words lowercase
    /// </summary>
    private string ProcessSentenceCase(string inputText)
    {
        var words = inputText.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        
        if (words.Length == 0)
        {
            return string.Empty;
        }

        var builder = new StringBuilder();
        builder.Append(words[0]);

        for (int i = 1; i < words.Length; i++)
        {
            builder.Append(' ');
            builder.Append(words[i].ToLowerInvariant());
        }

        return builder.ToString();
    }

    /// <summary>
    /// Truncates text to maximum allowed length
    /// </summary>
    private string TruncateToMaxLength(string text)
    {
        if (text.Length <= MaxOutputLength)
        {
            return text;
        }

        _logger.LogDebug(
            "Truncating text from {OriginalLength} to {MaxLength} characters",
            text.Length,
            MaxOutputLength);

        return text.Substring(0, MaxOutputLength);
    }

    /// <summary>
    /// Performs advanced processing including simulated delay and file output
    /// </summary>
    private async Task PerformAdvancedProcessingAsync(
        int taskId,
        string processedText,
        CancellationToken cancellationToken)
    {
        _logger.LogDebug(
            "Performing advanced processing for task {TaskId}",
            taskId);

        // Simulate processing delay
        await Task.Delay(ProcessingDelayMilliseconds, cancellationToken);

        // Write output using injected writer (separated concern)
        try
        {
            await _outputWriter.WriteTaskOutputAsync(taskId, processedText, cancellationToken);
            
            _logger.LogInformation(
                "Successfully wrote output for task {TaskId}",
                taskId);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to write output for task {TaskId}",
                taskId);
            throw;
        }
    }
}
