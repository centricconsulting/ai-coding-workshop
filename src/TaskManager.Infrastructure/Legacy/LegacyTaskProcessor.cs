namespace TaskManager.Infrastructure.Legacy;

/// <summary>
/// LEGACY CODE - This is intentionally bad code for refactoring exercise in Lab 3
/// Anti-patterns included:
/// - Long method with nested conditionals
/// - Poor naming
/// - No logging
/// - Synchronous code
/// - No error handling
/// - Mixed concerns
/// - Magic numbers and strings
/// </summary>
public class LegacyTaskProcessor
{
    public string ProcessTask(int id, string data, int type, bool flag)
    {
        var result = "";
        
        if (data != null)
        {
            if (data.Length > 0)
            {
                if (type == 1)
                {
                    if (flag)
                    {
                        for (int i = 0; i < data.Length; i++)
                        {
                            if (data[i] == ' ')
                            {
                                result += "_";
                            }
                            else
                            {
                                if (char.IsUpper(data[i]))
                                {
                                    result += char.ToLower(data[i]);
                                }
                                else
                                {
                                    result += char.ToUpper(data[i]);
                                }
                            }
                        }
                        
                        if (result.Length > 50)
                        {
                            result = result.Substring(0, 50);
                        }
                        
                        // Simulate some processing
                        System.Threading.Thread.Sleep(100);
                        
                        // Write to file (bad practice - mixed concerns)
                        try
                        {
                            System.IO.File.WriteAllText($"task_{id}.txt", result);
                        }
                        catch
                        {
                            // Swallow exception (bad practice)
                        }
                    }
                    else
                    {
                        result = data.ToUpper();
                    }
                }
                else if (type == 2)
                {
                    var words = data.Split(' ');
                    for (int i = 0; i < words.Length; i++)
                    {
                        if (i == 0)
                        {
                            result = words[i];
                        }
                        else
                        {
                            result += " " + words[i].ToLower();
                        }
                    }
                }
                else
                {
                    result = data;
                }
            }
        }
        
        return result;
    }
}

// TODO: During Lab 3, participants will use Copilot to refactor this into:
// - Multiple focused methods
// - Async implementation
// - Proper logging with ILogger
// - Guard clauses instead of nested ifs
// - Meaningful parameter and variable names
// - Proper error handling
// - Separation of concerns
