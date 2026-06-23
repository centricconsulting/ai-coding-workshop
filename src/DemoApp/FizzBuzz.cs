namespace DemoApp;

public sealed class FizzBuzz
{
    public string Evaluate(int number)
    {
        if (number % 15 == 0) return "FizzBuzz";
        if (number % 3 == 0) return "Fizz";
        if (number % 5 == 0) return "Buzz";
        return number.ToString();
    }
}

