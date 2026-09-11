namespace HotPizza.UI;

public interface IConsoleAdapter
{
    string ReadLine();
    void Write(string message);
    void WriteLine(string message = "");
}
