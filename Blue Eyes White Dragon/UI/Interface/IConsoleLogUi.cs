using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blue_Eyes_White_Dragon.UI.Interface
{
    public interface IConsoleLogUi
    {
        int GetConsoleLineNumber();
        void AppendConsoleText(string message);
        void RemoveOldestLine();

    }
}
