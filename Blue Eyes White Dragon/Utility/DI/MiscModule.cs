using System;
using Blue_Eyes_White_Dragon.DataAccess;
using Blue_Eyes_White_Dragon.DataAccess.Interface;
using Blue_Eyes_White_Dragon.Utility.Interface;
using Blue_Eyes_White_Dragon.Utility.Logging;
using Ninject.Modules;

namespace Blue_Eyes_White_Dragon.Utility.DI
{
    public class MiscModule : NinjectModule
    {
        public override void Load()
        {
            var kernel = Kernel ?? throw new ArgumentNullException(nameof(Kernel));

            kernel.Bind<ILogger>().To<ConsoleLogger>().InSingletonScope();
            kernel.Bind<ICardDbContext>().To<CardDbContext>();
        }
    }
}