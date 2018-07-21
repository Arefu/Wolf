using System;
using Blue_Eyes_White_Dragon.Presenter.Interface;
using Blue_Eyes_White_Dragon.UI;
using Blue_Eyes_White_Dragon.UI.Interface;
using Ninject.Modules;

namespace Blue_Eyes_White_Dragon.Misc.DI
{
    public class UiModule : NinjectModule
    {
        public override void Load()
        {
            var kernel = Kernel ?? throw new ArgumentNullException(nameof(Kernel));

            kernel.Bind<IArtworkEditor>().To<ArtworkEditor>().InSingletonScope();
            kernel.Bind<IArtworkPicker>().To<ArtworkPicker>();
        }
    }
}
