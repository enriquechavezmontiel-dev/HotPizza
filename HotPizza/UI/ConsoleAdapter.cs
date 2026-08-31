namespace HotPizza.UI;

public class ConsoleAdapter : IConsoleAdapter
{
    public string ReadLine()
    {
        return Console.ReadLine() ?? string.Empty;
    }

    public void Write(string message)
    {
        Console.Write(message);
    }

    public void WriteLine(string message = "")
    {
        Console.WriteLine(message);
    }
}
