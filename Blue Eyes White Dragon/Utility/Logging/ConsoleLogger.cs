using System;
using Blue_Eyes_White_Dragon.Utility.Interface;

namespace Blue_Eyes_White_Dragon.Utility.Logging
{
    public class ConsoleLogger : ILogger
    {
        public event Action<string> AppendTextToConsole;
        public event Action<string> AppendExceptionToConsole;

        public void LogInformation(string message)
        {
            var formattedMessage = Localization.MessageFormattedForConsole(message);
            AppendTextToConsole?.Invoke(formattedMessage);
        }

        public void LogException(Exception exception)
        {
            var message = Localization.Exception(exception.Message, exception.InnerException?.Message);
            AppendExceptionToConsole?.Invoke(message);
        }
    }
}
