using System;
using Blue_Eyes_White_Dragon.Business;
using Blue_Eyes_White_Dragon.Business.Interface;
using Ninject.Modules;

namespace Blue_Eyes_White_Dragon.Misc.DI
{
    public class LogicModule : NinjectModule
    {
        public override void Load()
        {
            var kernel = Kernel ?? throw new ArgumentNullException(nameof(Kernel));

            kernel.Bind<IArtworkEditorLogic>().To<ArtworkEditorLogic>().InSingletonScope();
            kernel.Bind<IArtworkPickerLogic>().To<ArtworkPickerLogic>();

            kernel.Bind<IArtworkManager>().To<ArtworkManager>().InSingletonScope();
        }
    }
}
