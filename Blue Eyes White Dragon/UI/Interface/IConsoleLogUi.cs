namespace Blue_Eyes_White_Dragon.UI.Interface
{
    public interface IConsoleLogUi
    {
        int GetConsoleLineNumber();
        void AppendConsoleText(string message);
        void RemoveOldestLine();
    }
}
