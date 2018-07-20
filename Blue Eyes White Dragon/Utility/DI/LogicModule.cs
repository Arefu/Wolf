using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blue_Eyes_White_Dragon.Business;
using Blue_Eyes_White_Dragon.Business.Interface;
using Ninject.Modules;

namespace Blue_Eyes_White_Dragon.Utility.DI
{
    public class LogicModule : NinjectModule
    {
        public override void Load()
        {
            var kernel = Kernel ?? throw new ArgumentNullException(nameof(Kernel));

            kernel.Bind<IArtworkEditorLogic>().To<ArtworkEditorLogic>();
            kernel.Bind<IArtworkPickerLogic>().To<ArtworkPickerLogic>();

            kernel.Bind<IArtworkManager>().To<ArtworkManager>();
        }
    }
}
