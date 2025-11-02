using FakeItEasy;
using Microsoft.Extensions.Logging;
using TaskManager.Infrastructure.Legacy;
using Xunit;

namespace TaskManager.UnitTests.Legacy.LegacyTaskProcessorTests;

public sealed class ProcessTaskAsyncTests
{
    private readonly ITaskOutputWriter _fakeOutputWriter;
    private readonly ILogger<LegacyTaskProcessor> _fakeLogger;
    private readonly LegacyTaskProcessor _processor;

    public ProcessTaskAsyncTests()
    {
        _fakeOutputWriter = A.Fake<ITaskOutputWriter>();
        _fakeLogger = A.Fake<ILogger<LegacyTaskProcessor>>();
        _processor = new LegacyTaskProcessor(_fakeOutputWriter, _fakeLogger);
    }

    [Fact]
    public async System.Threading.Tasks.Task ProcessTaskAsync_WithNullInput_ReturnsEmptyString()
    {
        // Arrange
        const int taskId = 1;
        const string? inputText = null;

        // Act
        var result = await _processor.ProcessTaskAsync(
            taskId,
            inputText!,
            TextProcessingType.ToggleCaseWithUnderscores,
            enableAdvancedProcessing: false);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public async System.Threading.Tasks.Task ProcessTaskAsync_WithEmptyInput_ReturnsEmptyString()
    {
        // Arrange
        const int taskId = 1;
        const string inputText = "";

        // Act
        var result = await _processor.ProcessTaskAsync(
            taskId,
            inputText,
            TextProcessingType.ToggleCaseWithUnderscores,
            enableAdvancedProcessing: false);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public async System.Threading.Tasks.Task ProcessTaskAsync_ToggleCaseWithoutAdvanced_ReturnsUppercase()
    {
        // Arrange
        const int taskId = 1;
        const string inputText = "Hello World";

        // Act
        var result = await _processor.ProcessTaskAsync(
            taskId,
            inputText,
            TextProcessingType.ToggleCaseWithUnderscores,
            enableAdvancedProcessing: false);

        // Assert
        Assert.Equal("HELLO WORLD", result);
    }

    [Fact]
    public async System.Threading.Tasks.Task ProcessTaskAsync_ToggleCaseWithAdvanced_TogglesAndReplacesSpaces()
    {
        // Arrange
        const int taskId = 1;
        const string inputText = "Hello World";

        // Act
        var result = await _processor.ProcessTaskAsync(
            taskId,
            inputText,
            TextProcessingType.ToggleCaseWithUnderscores,
            enableAdvancedProcessing: true);

        // Assert
        Assert.Equal("hELLO_wORLD", result);
        A.CallTo(() => _fakeOutputWriter.WriteTaskOutputAsync(
            taskId,
            "hELLO_wORLD",
            A<CancellationToken>._))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async System.Threading.Tasks.Task ProcessTaskAsync_ToggleCaseWithAdvanced_TruncatesAt50Characters()
    {
        // Arrange
        const int taskId = 1;
        var inputText = new string('A', 100); // 100 'A' characters

        // Act
        var result = await _processor.ProcessTaskAsync(
            taskId,
            inputText,
            TextProcessingType.ToggleCaseWithUnderscores,
            enableAdvancedProcessing: true);

        // Assert
        Assert.Equal(50, result.Length);
        Assert.Equal(new string('a', 50), result); // All lowercase 'a' after toggle
    }

    [Fact]
    public async System.Threading.Tasks.Task ProcessTaskAsync_SentenceCase_FirstWordUnchangedOthersLowercase()
    {
        // Arrange
        const int taskId = 1;
        const string inputText = "HELLO WORLD TEST";

        // Act
        var result = await _processor.ProcessTaskAsync(
            taskId,
            inputText,
            TextProcessingType.SentenceCase,
            enableAdvancedProcessing: false);

        // Assert
        Assert.Equal("HELLO world test", result);
    }

    [Fact]
    public async System.Threading.Tasks.Task ProcessTaskAsync_SentenceCase_WithSingleWord_ReturnsUnchanged()
    {
        // Arrange
        const int taskId = 1;
        const string inputText = "HELLO";

        // Act
        var result = await _processor.ProcessTaskAsync(
            taskId,
            inputText,
            TextProcessingType.SentenceCase,
            enableAdvancedProcessing: false);

        // Assert
        Assert.Equal("HELLO", result);
    }

    [Fact]
    public async System.Threading.Tasks.Task ProcessTaskAsync_None_ReturnsInputUnchanged()
    {
        // Arrange
        const int taskId = 1;
        const string inputText = "Hello World 123!";

        // Act
        var result = await _processor.ProcessTaskAsync(
            taskId,
            inputText,
            TextProcessingType.None,
            enableAdvancedProcessing: false);

        // Assert
        Assert.Equal(inputText, result);
    }

    [Fact]
    public async System.Threading.Tasks.Task ProcessTaskAsync_WithAdvancedProcessing_CallsOutputWriter()
    {
        // Arrange
        const int taskId = 42;
        const string inputText = "Test";

        // Act
        await _processor.ProcessTaskAsync(
            taskId,
            inputText,
            TextProcessingType.None,
            enableAdvancedProcessing: true);

        // Assert
        A.CallTo(() => _fakeOutputWriter.WriteTaskOutputAsync(
            taskId,
            inputText,
            A<CancellationToken>._))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async System.Threading.Tasks.Task ProcessTaskAsync_WithoutAdvancedProcessing_DoesNotCallOutputWriter()
    {
        // Arrange
        const int taskId = 42;
        const string inputText = "Test";

        // Act
        await _processor.ProcessTaskAsync(
            taskId,
            inputText,
            TextProcessingType.None,
            enableAdvancedProcessing: false);

        // Assert
        A.CallTo(() => _fakeOutputWriter.WriteTaskOutputAsync(
            A<int>._,
            A<string>._,
            A<CancellationToken>._))
            .MustNotHaveHappened();
    }

    [Fact]
    public async System.Threading.Tasks.Task ProcessTaskAsync_WhenOutputWriterThrows_PropagatesException()
    {
        // Arrange
        const int taskId = 1;
        const string inputText = "Test";
        var expectedException = new IOException("Disk full");

        A.CallTo(() => _fakeOutputWriter.WriteTaskOutputAsync(
            A<int>._,
            A<string>._,
            A<CancellationToken>._))
            .Throws(expectedException);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<IOException>(
            async () => await _processor.ProcessTaskAsync(
                taskId,
                inputText,
                TextProcessingType.None,
                enableAdvancedProcessing: true));

        Assert.Same(expectedException, exception);
    }

    [Fact]
    public async System.Threading.Tasks.Task ProcessTaskAsync_WithInvalidProcessingType_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        const int taskId = 1;
        const string inputText = "Test";
        const TextProcessingType invalidType = (TextProcessingType)999;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(
            async () => await _processor.ProcessTaskAsync(
                taskId,
                inputText,
                invalidType,
                enableAdvancedProcessing: false));
    }

    [Fact]
    public async System.Threading.Tasks.Task ProcessTaskAsync_PassesCancellationToken_ToOutputWriter()
    {
        // Arrange
        const int taskId = 1;
        const string inputText = "Test";
        var cts = new CancellationTokenSource();

        // Act
        await _processor.ProcessTaskAsync(
            taskId,
            inputText,
            TextProcessingType.None,
            enableAdvancedProcessing: true,
            cts.Token);

        // Assert
        A.CallTo(() => _fakeOutputWriter.WriteTaskOutputAsync(
            taskId,
            inputText,
            cts.Token))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async System.Threading.Tasks.Task ProcessTaskAsync_LogsInformation_OnSuccessfulProcessing()
    {
        // Arrange
        const int taskId = 1;
        const string inputText = "Test";

        // Act
        await _processor.ProcessTaskAsync(
            taskId,
            inputText,
            TextProcessingType.None,
            enableAdvancedProcessing: false);

        // Assert
        A.CallTo(_fakeLogger)
            .Where(call => 
                call.Method.Name == "Log" && 
                call.GetArgument<LogLevel>(0) == LogLevel.Information)
            .MustHaveHappened();
    }
}
