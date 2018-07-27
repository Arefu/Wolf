using System;
using System.IO;
using System.Reflection;
using Blue_Eyes_White_Dragon.DataAccess.Interface;
using Blue_Eyes_White_Dragon.DataAccess.Repository;
using Blue_Eyes_White_Dragon.Misc.Logging;
using Ninject;
using Ninject.Modules;
using Yu_Gi_Oh.File_Handling.Miscellaneous_Files;

namespace Blue_Eyes_White_Dragon.Misc.DI
{
    public class RepositoryModule : NinjectModule
    {
        public override void Load()
        {
            var kernel = Kernel ?? throw new ArgumentNullException(nameof(Kernel));

            kernel.Bind<ICardRepository>().To<CardRepository>();
            var assemblyDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            kernel.Bind<IFileRepository>().To<FileRepository>();
            //kernel.Bind<IFileRepository>().ToMethod(x => new FileRepository(kernel.Get<ConsoleLogger>()));
            kernel.Bind<IGameFileRepository>().ToMethod(x => new GameFileRepository(x.Kernel.Get<Manager>()));
            kernel.Bind<Manager>().ToMethod(x =>
            {
                var manager = new Manager();
                manager.Load();
                return manager;
            }).InSingletonScope();
            kernel.Bind<IResourceRepository>().To<ResourceRepository>();
            kernel.Bind<ISettingRepository>().To<SettingRepository>();
            kernel.Bind<ISaveAndLoadRepository>().ToMethod(x => new SaveAndAndLoadRepository(assemblyDir,x.Kernel.Get<ConsoleLogger>()));
            kernel.Bind<IImageRepository>().To<ImageRepository>();
        }
    }
}
