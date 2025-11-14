using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace iReverse_UniSPD_FRP.Modules
{
    /// <summary>
    /// Classe base abstrata para módulos de marca
    /// </summary>
    public abstract class ModuleBase : IBrandModule, IDisposable
    {
        protected bool _disposed = false;
        protected UserControl _view;
        protected IModuleViewModel _viewModel;

        /// <summary>
        /// Nome da marca
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// View do módulo
        /// </summary>
        public UserControl View
        {
            get
            {
                if (_view == null)
                {
                    _view = CreateView();
                }
                return _view;
            }
        }

        /// <summary>
        /// ViewModel do módulo
        /// </summary>
        public IModuleViewModel ViewModel
        {
            get
            {
                if (_viewModel == null)
                {
                    _viewModel = CreateViewModel();
                }
                return _viewModel;
            }
        }

        /// <summary>
        /// Cria a View do módulo
        /// </summary>
        protected abstract UserControl CreateView();

        /// <summary>
        /// Cria o ViewModel do módulo
        /// </summary>
        protected abstract IModuleViewModel CreateViewModel();

        /// <summary>
        /// Inicializa o módulo
        /// </summary>
        public virtual async Task Initialize()
        {
            if (_viewModel != null)
            {
                await _viewModel.Initialize();
            }
        }

        /// <summary>
        /// Libera recursos
        /// </summary>
        public virtual void Dispose()
        {
            if (!_disposed)
            {
                _viewModel?.CancelOperation();
                _view?.Dispose();
                _view = null;
                _viewModel = null;
                _disposed = true;
            }
        }
    }
}

