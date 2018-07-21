using System;
using Blue_Eyes_White_Dragon.Business.Factory;
using Blue_Eyes_White_Dragon.Business.Interface;
using Ninject.Modules;

namespace Blue_Eyes_White_Dragon.Misc.DI
{
    public class FactoryModule : NinjectModule
    {
        public override void Load()
        {
            var kernel = Kernel ?? throw new ArgumentNullException(nameof(Kernel));

            kernel.Bind<IArtworkPickerPresenterFactory>().ToMethod(x => new ArtworkPickerPresenterFactory(kernel));
        }
    }
}
