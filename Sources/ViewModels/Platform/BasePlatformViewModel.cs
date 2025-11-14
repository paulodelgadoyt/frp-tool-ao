using System;
using System.Windows.Forms;
using iReverse_UniSPD_FRP.Modules;

namespace iReverse_UniSPD_FRP.ViewModels.Platform
{
    /// <summary>
    /// ViewModel base para plataformas
    /// </summary>
    public abstract class BasePlatformViewModel : IModuleViewModel
    {
        protected UserControl _view;
        protected bool _isOperationRunning;

        public abstract string BrandName { get; }
        public abstract string PlatformName { get; }

        public System.Collections.ObjectModel.ObservableCollection<IModuleCommand> Commands { get; protected set; }

        public bool IsOperationRunning
        {
            get => _isOperationRunning;
            set
            {
                if (_isOperationRunning != value)
                {
                    _isOperationRunning = value;
                    OperationRunningChanged?.Invoke(this, value);
                }
            }
        }

        public event EventHandler<bool> OperationRunningChanged;

        /// <summary>
        /// View (UserControl) da plataforma
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
        /// Cria a View da plataforma
        /// </summary>
        protected abstract UserControl CreateView();

        public abstract System.Threading.Tasks.Task Initialize();

        public abstract void CancelOperation();
    }
}

