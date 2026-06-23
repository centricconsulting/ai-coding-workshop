namespace TaskManager.UnitTests;

public sealed class FizzBuzzTests
{
    private readonly DemoApp.FizzBuzz _sut = new();

    [Theory]
    [InlineData(3)]
    [InlineData(6)]
    [InlineData(9)]
    public void Evaluate_MultipleOfThree_ReturnsFizz(int number)
    {
        var result = _sut.Evaluate(number);

        Assert.Equal("Fizz", result);
    }

    [Theory]
    [InlineData(5)]
    [InlineData(10)]
    [InlineData(20)]
    public void Evaluate_MultipleOfFive_ReturnsBuzz(int number)
    {
        var result = _sut.Evaluate(number);

        Assert.Equal("Buzz", result);
    }

    [Theory]
    [InlineData(15)]
    [InlineData(30)]
    [InlineData(45)]
    public void Evaluate_MultipleOfBothThreeAndFive_ReturnsFizzBuzz(int number)
    {
        var result = _sut.Evaluate(number);

        Assert.Equal("FizzBuzz", result);
    }

    [Theory]
    [InlineData(1, "1")]
    [InlineData(2, "2")]
    [InlineData(7, "7")]
    public void Evaluate_NotMultipleOfThreeOrFive_ReturnsNumberAsString(int number, string expected)
    {
        var result = _sut.Evaluate(number);

        Assert.Equal(expected, result);
    }
}
