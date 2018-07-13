using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blue_Eyes_White_Dragon.Utility.Interface;

namespace Blue_Eyes_White_Dragon.Utility
{
    public class Logger : ILogger
    {
        public void LogInformation(string message)
        {
            Debug.WriteLine(message);
        }
    }
}
