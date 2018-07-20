using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blue_Eyes_White_Dragon.Utility.Interface
{
    public interface ILogger
    {
        void LogInformation(string message);
        void LogException(Exception e);
        event Action<string> AppendTextToConsole;
        event Action<string> AppendExceptionToConsole;
    }
}
