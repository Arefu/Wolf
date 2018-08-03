using Blue_Eyes_White_Dragon.Business.Interface;
using Blue_Eyes_White_Dragon.Presenter;
using Blue_Eyes_White_Dragon.UI;
using Blue_Eyes_White_Dragon.UI.Interface;
using Ninject;

namespace Blue_Eyes_White_Dragon.Business.Factory
{
    public class ArtworkPickerPresenterFactory : IArtworkPickerPresenterFactory
    {
        private readonly IKernel _kernel;

        public ArtworkPickerPresenterFactory(IKernel kernel)
        {
            _kernel = kernel;
        }

        public IArtworkPickerPresenter NewArtworkPickerPresenter()
        {
            return new ArtworkPickerPresenter(_kernel.Get<ArtworkPicker>(), _kernel.Get<ArtworkPickerLogic>());
        }
    }
}