using System;
using System.Windows.Forms;
using Blue_Eyes_White_Dragon.Misc.DI;
using Blue_Eyes_White_Dragon.Presenter;
using Ninject;

namespace Blue_Eyes_White_Dragon
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            IKernel kernel = new StandardKernel(new MiscModule(), new FactoryModule(), new LogicModule(), new PresenterModule(), new RepositoryModule(), new UiModule());

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var presenter = kernel.Get<ArtworkEditorPresenter>();
            Application.Run((Form)presenter.View);
        }
    }
}
