using System;

namespace Blue_Eyes_White_Dragon.Misc.Interface
{
    public interface ILogger
    {
        event Action<string> AppendTextToConsole;
        event Action<string> AppendExceptionToConsole;
        void LogInformation(string message);
        void LogException(Exception e);
        void LogError(string message);
    }
}
