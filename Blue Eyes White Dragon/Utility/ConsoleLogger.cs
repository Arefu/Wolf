using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blue_Eyes_White_Dragon.Business;
using Blue_Eyes_White_Dragon.Presenter.Interface;
using Blue_Eyes_White_Dragon.UI.Interface;
using Blue_Eyes_White_Dragon.Utility.Interface;

namespace Blue_Eyes_White_Dragon.Utility
{
    public class ConsoleLogger : ILogger
    {
        private readonly IArtworkEditorPresenter _artworkEditorPresenter;

        public ConsoleLogger(IArtworkEditorPresenter artworkEditorPresenter)
        {
            _artworkEditorPresenter = artworkEditorPresenter;
        }

        public void LogInformation(string message)
        {
            var textLength = _artworkEditorPresenter.GetConsoleLineNumber();
            ClipTextBox(textLength);

            var newline = $"\r\n";
            var formattedMessage = $"{message}{newline}";
            _artworkEditorPresenter.AppendConsoleText(formattedMessage);

        }
        private void ClipTextBox(int allText)
        {
            if (allText > Constants.ConsoleLimit)
            {
                _artworkEditorPresenter.RemoveOldestLine();
            }
        }
    }
}
