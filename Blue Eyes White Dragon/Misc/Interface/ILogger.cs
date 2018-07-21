using System;

namespace Blue_Eyes_White_Dragon.Misc.Interface
{
    public interface ILogger
    {
        void LogInformation(string message);
        void LogException(Exception e);
        event Action<string> AppendTextToConsole;
        event Action<string> AppendExceptionToConsole;
        void LogError(string message);
    }
}
