using FakeItEasy;
using Microsoft.Extensions.Logging;
using TaskManager.Infrastructure.Legacy;
using Xunit;

namespace TaskManager.UnitTests.Legacy.FileTaskOutputWriterTests;

public sealed class WriteTaskOutputAsyncTests
{
    private readonly ILogger<FileTaskOutputWriter> _fakeLogger;
    private readonly FileTaskOutputWriter _writer;

    public WriteTaskOutputAsyncTests()
    {
        _fakeLogger = A.Fake<ILogger<FileTaskOutputWriter>>();
        _writer = new FileTaskOutputWriter(_fakeLogger);
    }

    [Fact]
    public async System.Threading.Tasks.Task WriteTaskOutputAsync_WithNullContent_ThrowsArgumentNullException()
    {
        // Arrange
        const int taskId = 1;
        const string? content = null;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(
            async () => await _writer.WriteTaskOutputAsync(taskId, content!, CancellationToken.None));
    }

    [Fact]
    public async System.Threading.Tasks.Task WriteTaskOutputAsync_WithValidContent_CreatesFileSuccessfully()
    {
        // Arrange
        const int taskId = 42;
        const string content = "Test content";
        var expectedFileName = $"task_{taskId}.txt";

        try
        {
            // Act
            await _writer.WriteTaskOutputAsync(taskId, content, CancellationToken.None);

            // Assert
            Assert.True(File.Exists(expectedFileName));
            var fileContent = await File.ReadAllTextAsync(expectedFileName);
            Assert.Equal(content, fileContent);
        }
        finally
        {
            // Cleanup
            if (File.Exists(expectedFileName))
            {
                File.Delete(expectedFileName);
            }
        }
    }

    [Fact]
    public async System.Threading.Tasks.Task WriteTaskOutputAsync_LogsDebug_BeforeWriting()
    {
        // Arrange
        const int taskId = 1;
        const string content = "Test";
        var fileName = $"task_{taskId}.txt";

        try
        {
            // Act
            await _writer.WriteTaskOutputAsync(taskId, content, CancellationToken.None);

            // Assert
            A.CallTo(_fakeLogger)
                .Where(call => 
                    call.Method.Name == "Log" && 
                    call.GetArgument<LogLevel>(0) == LogLevel.Debug)
                .MustHaveHappened();
        }
        finally
        {
            // Cleanup
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
        }
    }

    [Fact]
    public async System.Threading.Tasks.Task WriteTaskOutputAsync_LogsInformation_OnSuccess()
    {
        // Arrange
        const int taskId = 1;
        const string content = "Test";
        var fileName = $"task_{taskId}.txt";

        try
        {
            // Act
            await _writer.WriteTaskOutputAsync(taskId, content, CancellationToken.None);

            // Assert
            A.CallTo(_fakeLogger)
                .Where(call => 
                    call.Method.Name == "Log" && 
                    call.GetArgument<LogLevel>(0) == LogLevel.Information)
                .MustHaveHappened();
        }
        finally
        {
            // Cleanup
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
        }
    }

    [Fact]
    public async System.Threading.Tasks.Task WriteTaskOutputAsync_WithCancellationToken_RespectsCancellation()
    {
        // Arrange
        const int taskId = 1;
        const string content = "Test";
        var cts = new CancellationTokenSource();
        cts.Cancel(); // Cancel immediately

        // Act & Assert
        await Assert.ThrowsAnyAsync<OperationCanceledException>(
            async () => await _writer.WriteTaskOutputAsync(taskId, content, cts.Token));
    }

    [Fact]
    public async System.Threading.Tasks.Task WriteTaskOutputAsync_OverwritesExistingFile()
    {
        // Arrange
        const int taskId = 99;
        const string initialContent = "Initial content";
        const string updatedContent = "Updated content";
        var fileName = $"task_{taskId}.txt";

        try
        {
            // Write initial content
            await _writer.WriteTaskOutputAsync(taskId, initialContent, CancellationToken.None);
            
            // Act - Write updated content
            await _writer.WriteTaskOutputAsync(taskId, updatedContent, CancellationToken.None);

            // Assert
            var fileContent = await File.ReadAllTextAsync(fileName);
            Assert.Equal(updatedContent, fileContent);
        }
        finally
        {
            // Cleanup
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
        }
    }

    [Fact]
    public async System.Threading.Tasks.Task WriteTaskOutputAsync_WithEmptyContent_CreatesEmptyFile()
    {
        // Arrange
        const int taskId = 100;
        const string content = "";
        var fileName = $"task_{taskId}.txt";

        try
        {
            // Act
            await _writer.WriteTaskOutputAsync(taskId, content, CancellationToken.None);

            // Assert
            Assert.True(File.Exists(fileName));
            var fileContent = await File.ReadAllTextAsync(fileName);
            Assert.Equal(string.Empty, fileContent);
        }
        finally
        {
            // Cleanup
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
        }
    }
}
