using FakeItEasy;
using Microsoft.Extensions.Logging;
using TaskManager.Application.Ports;
using TaskManager.Infrastructure.Legacy;

namespace TaskManager.UnitTests.Legacy.LegacyTaskProcessorTests;

/// <summary>
/// Characterization tests that encode the observable behavior of TaskProcessor.ProcessTaskAsync.
/// </summary>
public sealed class ProcessTaskTests
{
    private readonly ITaskFileWriter _fileWriter = A.Fake<ITaskFileWriter>();
    private readonly TaskProcessor _sut;

    public ProcessTaskTests()
    {
        var logger = A.Fake<ILogger<TaskProcessor>>();
        _sut = new TaskProcessor(logger, _fileWriter);
    }

    // --- Guard clauses ---

    [Fact]
    public async Task ProcessTaskAsync_NullData_ThrowsArgumentNullException()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(() =>
            _sut.ProcessTaskAsync(taskId: 1, data: null!, type: ProcessingType.Uppercase,
                cancellationToken: TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task ProcessTaskAsync_EmptyData_ThrowsArgumentException()
    {
        await Assert.ThrowsAsync<ArgumentException>(() =>
            _sut.ProcessTaskAsync(taskId: 1, data: "", type: ProcessingType.Uppercase,
                cancellationToken: TestContext.Current.CancellationToken));
    }

    // --- Uppercase ---

    [Fact]
    public async Task ProcessTaskAsync_Uppercase_ReturnsFullyUppercasedData()
    {
        var result = await _sut.ProcessTaskAsync(taskId: 1, data: "Hello World", type: ProcessingType.Uppercase,
            cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal("HELLO WORLD", result);
    }

    [Fact]
    public async Task ProcessTaskAsync_Uppercase_AlreadyUppercase_ReturnsSameValue()
    {
        var result = await _sut.ProcessTaskAsync(taskId: 1, data: "ALREADY UPPER", type: ProcessingType.Uppercase,
            cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal("ALREADY UPPER", result);
    }

    // --- NormalizeAndInvertCase ---

    [Fact]
    public async Task ProcessTaskAsync_NormalizeAndInvertCase_InvertsCaseAndReplacesSpacesWithUnderscores()
    {
        var result = await _sut.ProcessTaskAsync(taskId: 1, data: "Hello World", type: ProcessingType.NormalizeAndInvertCase,
            cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal("hELLO_wORLD", result);
    }

    [Fact]
    public async Task ProcessTaskAsync_NormalizeAndInvertCase_ResultLongerThan50Chars_TruncatesTo50()
    {
        var input = new string('a', 52);

        var result = await _sut.ProcessTaskAsync(taskId: 1, data: input, type: ProcessingType.NormalizeAndInvertCase,
            cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal(50, result.Length);
        Assert.Equal(new string('A', 50), result);
    }

    [Fact]
    public async Task ProcessTaskAsync_NormalizeAndInvertCase_ResultExactly50Chars_IsNotTruncated()
    {
        var input = new string('a', 50);

        var result = await _sut.ProcessTaskAsync(taskId: 1, data: input, type: ProcessingType.NormalizeAndInvertCase,
            cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal(50, result.Length);
    }

    [Fact]
    public async Task ProcessTaskAsync_NormalizeAndInvertCase_ResultShorterThan50Chars_IsNotTruncated()
    {
        var result = await _sut.ProcessTaskAsync(taskId: 1, data: "abc", type: ProcessingType.NormalizeAndInvertCase,
            cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal("ABC", result);
    }

    [Fact]
    public async Task ProcessTaskAsync_NormalizeAndInvertCase_WritesProcessedResultToFileWriter()
    {
        await _sut.ProcessTaskAsync(taskId: 42, data: "hello", type: ProcessingType.NormalizeAndInvertCase,
            cancellationToken: TestContext.Current.CancellationToken);

        A.CallTo(() => _fileWriter.WriteAsync(42, "HELLO", A<CancellationToken>._))
            .MustHaveHappenedOnceExactly();
    }

    // --- SentenceCase ---

    [Fact]
    public async Task ProcessTaskAsync_SentenceCase_KeepsFirstWordAndLowercasesRemainingWords()
    {
        var result = await _sut.ProcessTaskAsync(taskId: 1, data: "Hello WORLD foo", type: ProcessingType.SentenceCase,
            cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal("Hello world foo", result);
    }

    [Fact]
    public async Task ProcessTaskAsync_SentenceCase_SingleWord_ReturnsWordUnchanged()
    {
        var result = await _sut.ProcessTaskAsync(taskId: 1, data: "HELLO", type: ProcessingType.SentenceCase,
            cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal("HELLO", result);
    }

    [Fact]
    public async Task ProcessTaskAsync_SentenceCase_FirstWordAlreadyLower_RemainsLower()
    {
        var result = await _sut.ProcessTaskAsync(taskId: 1, data: "fix IT NOW", type: ProcessingType.SentenceCase,
            cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal("fix it now", result);
    }

    // --- Passthrough ---

    [Fact]
    public async Task ProcessTaskAsync_Passthrough_ReturnsDataUnchanged()
    {
        var result = await _sut.ProcessTaskAsync(taskId: 1, data: "unchanged", type: ProcessingType.Passthrough,
            cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal("unchanged", result);
    }
}

