using System.Windows.Forms;
using iReverse_UniSPD_FRP.Modules;

namespace iReverse_UniSPD_FRP.Modules.Samsung
{
    /// <summary>
    /// Módulo Samsung
    /// Exemplo simplificado usando BrandModuleBase
    /// </summary>
    public class SamsungModule : BrandModuleBase
    {
        private readonly SamsungViewModel _viewModel = new SamsungViewModel();
        private SamsungView _view;

        public override string Name => "Samsung";

        public override UserControl View
        {
            get
            {
                if (_view == null)
                {
                    _view = new SamsungView();
                    _view.SetViewModel(_viewModel);
                }
                return _view;
            }
        }

        public override IModuleViewModel ViewModel => _viewModel;
    }
}

