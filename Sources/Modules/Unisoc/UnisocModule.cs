using System.Windows.Forms;
using iReverse_UniSPD_FRP.Modules;

namespace iReverse_UniSPD_FRP.Modules.Unisoc
{
    /// <summary>
    /// Módulo Unisoc/Spreadtrum
    /// Exemplo simplificado usando BrandModuleBase
    /// </summary>
    public class UnisocModule : BrandModuleBase
    {
        private readonly UnisocViewModel _viewModel = new UnisocViewModel();
        private UnisocView _view;

        public override string Name => "Unisoc";

        public override UserControl View
        {
            get
            {
                if (_view == null)
                {
                    _view = new UnisocView();
                    _view.SetViewModel(_viewModel);
                }
                return _view;
            }
        }

        public override IModuleViewModel ViewModel => _viewModel;
    }
}

