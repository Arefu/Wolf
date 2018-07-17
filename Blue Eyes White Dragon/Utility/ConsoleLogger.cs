using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blue_Eyes_White_Dragon.Business;
using Blue_Eyes_White_Dragon.UI.Interface;
using Blue_Eyes_White_Dragon.Utility.Interface;

namespace Blue_Eyes_White_Dragon.Utility
{
    public class ConsoleLogger : ILogger
    {
        private readonly IArtworkEditor _artworkEditorUi;

        public ConsoleLogger(IArtworkEditor artworkEditorUi)
        {
            _artworkEditorUi = artworkEditorUi;
        }

        public void LogInformation(string message)
        {
            var textLength = _artworkEditorUi.GetConsoleLineNumber();
            ClipTextBox(textLength);

            var newline = $"\r\n";
            var formattedMessage = $"{message}{newline}";
            _artworkEditorUi.AppendConsoleText(formattedMessage);

        }
        private void ClipTextBox(int allText)
        {
            if (allText > Constants.ConsoleLimit)
            {
                _artworkEditorUi.RemoveOldestLine();
            }
        }
    }
}
